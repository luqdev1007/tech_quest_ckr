using System.Threading;
using Cysharp.Threading.Tasks;

namespace TestTask.Core.Network
{
    public interface IWebRequest<T>
    {
        UniTask<T> Execute(CancellationToken ct);
    }
}
