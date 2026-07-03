using System.Threading;
using Cysharp.Threading.Tasks;
using TestTask.Configs;
using TestTask.Core.Network;
using TestTask.Features.Breeds.Dto;

namespace TestTask.Features.Breeds
{
    public sealed class BreedsListRequest : IWebRequest<BreedsListResponse>
    {
        private readonly IWebRequest<BreedsListResponse> _inner;

        public BreedsListRequest(BreedsConfig config)
        {
            _inner = new JsonGetRequest<BreedsListResponse>($"{config.BaseUrl}/breeds");
        }

        public UniTask<BreedsListResponse> Execute(CancellationToken ct) => _inner.Execute(ct);
    }
}
