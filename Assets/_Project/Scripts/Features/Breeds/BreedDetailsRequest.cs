using System.Threading;
using Cysharp.Threading.Tasks;
using TestTask.Configs;
using TestTask.Core.Network;
using TestTask.Features.Breeds.Dto;

namespace TestTask.Features.Breeds
{
    public sealed class BreedDetailsRequest : IWebRequest<BreedResponse>
    {
        private readonly IWebRequest<BreedResponse> _inner;

        public BreedDetailsRequest(BreedsConfig config, string breedId)
        {
            _inner = new JsonGetRequest<BreedResponse>($"{config.BaseUrl}/breeds/{breedId}");
        }

        public UniTask<BreedResponse> Execute(CancellationToken ct) => _inner.Execute(ct);
    }
}
