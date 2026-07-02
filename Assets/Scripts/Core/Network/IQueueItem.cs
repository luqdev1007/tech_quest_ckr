using System.Threading;
using Cysharp.Threading.Tasks;

namespace TestTask.Core.Network
{
    internal interface IQueueItem
    {
        UniTask RunAsync(CancellationToken workerCt);
        void OnQueueDisposed();
    }
}
