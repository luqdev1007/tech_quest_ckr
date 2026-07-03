using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace TestTask.Core.Network
{
    public sealed class RequestQueue : IRequestQueue, IInitializable, IDisposable
    {
        private readonly Queue<IQueueItem> _items = new Queue<IQueueItem>();
        private readonly CancellationTokenSource _lifetime = new CancellationTokenSource();

        private UniTaskCompletionSource _wake;
        private bool _disposed;

        public void Initialize()
        {
            RunWorker(_lifetime.Token).Forget();
        }

        public IRequestHandle<T> Enqueue<T>(IWebRequest<T> request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (_disposed)
                throw new ObjectDisposedException(nameof(RequestQueue));

            var handle = new RequestHandle<T>(request);
            _items.Enqueue(handle);
            Debug.Log($"[RequestQueue] enqueued {typeof(T).Name}");
            WakeWorker();
            return handle;
        }

        private async UniTaskVoid RunWorker(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                IQueueItem item = _items.Count > 0 ? _items.Dequeue() : null;

                if (item == null)
                {
                    _wake = new UniTaskCompletionSource();
                    using (ct.Register(() => _wake?.TrySetCanceled()))
                    {
                        try
                        {
                            await _wake.Task;
                        }
                        catch (OperationCanceledException)
                        {
                            // позже
                        }
                    }

                    _wake = null;
                    continue;
                }

                await item.RunAsync(ct);
            }
        }

        private void WakeWorker()
        {
            var wake = _wake;
            _wake = null;
            wake?.TrySetResult();
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            _lifetime.Cancel();
            WakeWorker();

            while (_items.Count > 0)
                _items.Dequeue().OnQueueDisposed();

            _lifetime.Dispose();
        }
    }
}
