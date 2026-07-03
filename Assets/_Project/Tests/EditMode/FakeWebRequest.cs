using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestTask.Core.Network;

namespace TestTask.Tests.EditMode
{
    internal sealed class FakeWebRequest<T> : IWebRequest<T>
    {
        private readonly UniTaskCompletionSource<T> _tcs = new UniTaskCompletionSource<T>();

        public bool Started { get; private set; }

        public UniTask<T> Execute(CancellationToken ct)
        {
            Started = true;

            ct.Register(() => _tcs.TrySetCanceled(ct));
            return _tcs.Task;
        }

        public void Complete(T result) => _tcs.TrySetResult(result);

        public void Fail(Exception error) => _tcs.TrySetException(error);
    }
}
