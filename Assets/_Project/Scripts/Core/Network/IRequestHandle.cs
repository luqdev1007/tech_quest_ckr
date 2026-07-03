using Cysharp.Threading.Tasks;

namespace TestTask.Core.Network
{
    public interface IRequestHandle<T>
    {
        UniTask<T> Task { get; }
        void Cancel();
    }
}
