using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TestTask.Core.Network
{
    internal sealed class RequestHandle<T> : IRequestHandle<T>, IQueueItem
    {
        private readonly IWebRequest<T> _request;
        private readonly UniTaskCompletionSource<T> _completion = new UniTaskCompletionSource<T>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private bool _started;
        private bool _finished;

        public RequestHandle(IWebRequest<T> request)
        {
            _request = request;
        }

        public UniTask<T> Task => _completion.Task;

        public async UniTask RunAsync(CancellationToken workerCt)
        {
            if (_finished || _cts.IsCancellationRequested)
            {
                Finish();
                _completion.TrySetCanceled();
                return;
            }

            _started = true;
            Debug.Log($"[RequestQueue] executing {typeof(T).Name}");
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, workerCt);
            try
            {
                var result = await _request.Execute(linked.Token);
                Finish();
                _completion.TrySetResult(result);
            }
            catch (OperationCanceledException)
            {
                Finish();
                _completion.TrySetCanceled();
            }
            catch (Exception ex)
            {
                Finish();
                Debug.LogWarning($"[RequestQueue] request failed: {ex.Message}");
                _completion.TrySetException(ex);
            }
        }

        public void Cancel()
        {
            if (_finished)
                return;

            if (_started)
                Debug.Log($"[RequestQueue] aborting in-flight {typeof(T).Name}");
            else
                Debug.Log($"[RequestQueue] removed from queue (was pending) {typeof(T).Name}");

            _cts.Cancel();

            if (!_started)
            {
                Finish();
                _completion.TrySetCanceled();
            }
        }

        public void OnQueueDisposed()
        {
            if (_finished)
                return;

            Finish();
            _completion.TrySetCanceled();
        }

        private void Finish()
        {
            _finished = true;
        }
    }
}
