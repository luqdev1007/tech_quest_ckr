using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace TestTask.Core.Network
{
    public sealed class RequestQueueSmokeTest : MonoBehaviour
    {
        [Inject] private IRequestQueue _queue;

        private void Start()
        {
            RunAll(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid RunAll(CancellationToken ct)
        {
            await SequentialExecution(ct);
            await CancelBeforeStart(ct);
            await CancelInFlight(ct);
            await ErrorKeepsWorkerAlive(ct);
            Debug.Log("[SmokeTest] All scenarios finished. Review the log above.");
        }

        private async UniTask SequentialExecution(CancellationToken ct)
        {
            Debug.Log("[SmokeTest] (1) Sequential execution — enqueue two 1s requests.");

            var first = _queue.Enqueue(new DelayRequest("A", 1f, log: true));
            var second = _queue.Enqueue(new DelayRequest("B", 1f, log: true));

            await first.Task;
            await second.Task;

            Debug.Log("[SmokeTest] (1) OK — B started only after A finished (see timestamps).");
        }

        private async UniTask CancelBeforeStart(CancellationToken ct)
        {
            Debug.Log("[SmokeTest] (2) Cancel before start.");

            var blocker = _queue.Enqueue(new DelayRequest("blocker", 1.5f, log: true));
            var victim = _queue.Enqueue(new DelayRequest("victim-should-not-run", 1f, log: true));

            victim.Cancel();     
            victim.Cancel();     

            try
            {
                await victim.Task;
                Debug.LogError("[SmokeTest] (2) FAIL — victim task completed instead of cancelling.");
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[SmokeTest] (2) OK — victim threw OperationCanceledException and never ran.");
            }

            await blocker.Task; 
        }

        private async UniTask CancelInFlight(CancellationToken ct)
        {
            Debug.Log("[SmokeTest] (3) Cancel in flight.");

            var running = _queue.Enqueue(new DelayRequest("long-running", 5f, log: true));
            var next = _queue.Enqueue(new DelayRequest("after-cancel", 1f, log: true));

            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: ct);
            running.Cancel();

            try
            {
                await running.Task;
                Debug.LogError("[SmokeTest] (3) FAIL — running task completed instead of cancelling.");
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[SmokeTest] (3) OK — running request aborted mid-flight.");
            }

            await next.Task;
            Debug.Log("[SmokeTest] (3) OK — queue continued and ran 'after-cancel'.");
        }

        private async UniTask ErrorKeepsWorkerAlive(CancellationToken ct)
        {
            Debug.Log("[SmokeTest] (4) Error keeps worker alive.");

            var failing = _queue.Enqueue(new DelayRequest("failing", 0.5f, fail: true));
            var survivor = _queue.Enqueue(new DelayRequest("survivor", 0.5f, log: true));

            try
            {
                await failing.Task;
                Debug.LogError("[SmokeTest] (4) FAIL — failing task did not throw.");
            }
            catch (Exception ex)
            {
                Debug.Log($"[SmokeTest] (4) OK — failing task threw ({ex.Message}); worker still alive.");
            }

            await survivor.Task;
            Debug.Log("[SmokeTest] (4) OK — worker ran 'survivor' after the failure.");
        }

        private sealed class DelayRequest : IWebRequest<string>
        {
            private readonly string _name;
            private readonly float _seconds;
            private readonly bool _fail;
            private readonly bool _log;

            public DelayRequest(string name, float seconds, bool fail = false, bool log = false)
            {
                _name = name;
                _seconds = seconds;
                _fail = fail;
                _log = log;
            }

            public async UniTask<string> Execute(CancellationToken ct)
            {
                if (_log)
                    Debug.Log($"[SmokeTest]   -> START '{_name}' at t={Time.realtimeSinceStartup:0.00}");

                await UniTask.Delay(TimeSpan.FromSeconds(_seconds), cancellationToken: ct);

                if (_fail)
                    throw new WebRequestException($"fake failure for '{_name}'", 500);

                if (_log)
                    Debug.Log($"[SmokeTest]   -> DONE  '{_name}' at t={Time.realtimeSinceStartup:0.00}");

                return _name;
            }
        }
    }
}
