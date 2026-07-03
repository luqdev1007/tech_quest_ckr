using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TestTask.Core.Network;
using UnityEngine.TestTools;

namespace TestTask.Tests.EditMode
{
    public sealed class RequestQueueTests
    {
        private RequestQueue _queue;

        [SetUp]
        public void SetUp()
        {
            _queue = new RequestQueue();
            _queue.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _queue.Dispose();
        }

        [UnityTest]
        public IEnumerator Requests_ExecuteSequentially() => UniTask.ToCoroutine(async () =>
        {
            var f1 = new FakeWebRequest<int>();
            var f2 = new FakeWebRequest<int>();

            var h1 = _queue.Enqueue(f1);
            var h2 = _queue.Enqueue(f2);

            Assert.IsTrue(f1.Started, "first request must start immediately");
            Assert.IsFalse(f2.Started, "second request must wait for the first");

            f1.Complete(1);

            Assert.IsTrue(f2.Started, "second request must start once the first finishes");

            f2.Complete(2);

            Assert.AreEqual(1, await h1.Task);
            Assert.AreEqual(2, await h2.Task);
        });

        [UnityTest]
        public IEnumerator Cancel_BeforeStart_NeverExecutes() => UniTask.ToCoroutine(async () =>
        {
            var blocker = new FakeWebRequest<int>();
            var pending = new FakeWebRequest<int>();

            _queue.Enqueue(blocker);        
            var h = _queue.Enqueue(pending); 

            h.Cancel();

            Assert.IsFalse(pending.Started, "a request cancelled while pending must never execute");
            await AssertCanceled(h.Task);

            blocker.Complete(0);           
        });

        [UnityTest]
        public IEnumerator Cancel_InFlight_AbortsAndWorkerSurvives() => UniTask.ToCoroutine(async () =>
        {
            var inFlight = new FakeWebRequest<int>();
            var h = _queue.Enqueue(inFlight);

            Assert.IsTrue(inFlight.Started);

            h.Cancel();
            await AssertCanceled(h.Task);

            var next = new FakeWebRequest<int>();
            var hNext = _queue.Enqueue(next);

            Assert.IsTrue(next.Started, "worker must keep processing after an in-flight cancel");

            next.Complete(42);
            Assert.AreEqual(42, await hNext.Task);
        });

        [UnityTest]
        public IEnumerator Failure_FailsOnlyThatRequest_WorkerSurvives() => UniTask.ToCoroutine(async () =>
        {
            var bad = new FakeWebRequest<int>();
            var good = new FakeWebRequest<int>();

            var hBad = _queue.Enqueue(bad);
            var hGood = _queue.Enqueue(good);

            var error = new InvalidOperationException("boom");
            bad.Fail(error);

            Exception caught = null;
            try
            {
                await hBad.Task;
            }
            catch (Exception ex)
            {
                caught = ex;
            }

            Assert.AreSame(error, caught, "the failing request's Task must surface its own exception");
            Assert.IsTrue(good.Started, "worker must survive a failed request");

            good.Complete(7);
            Assert.AreEqual(7, await hGood.Task);
        });

        private static async UniTask AssertCanceled<T>(UniTask<T> task)
        {
            var canceled = false;
            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                canceled = true;
            }

            Assert.IsTrue(canceled, "expected OperationCanceledException");
        }
    }
}
