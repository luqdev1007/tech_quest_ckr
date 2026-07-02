using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace TestTask.Core.Network
{
    public sealed class TextureGetRequest : IWebRequest<Texture2D>
    {
        private readonly string _url;

        public TextureGetRequest(string url)
        {
            _url = url;
        }

        public async UniTask<Texture2D> Execute(CancellationToken ct)
        {
            using var request = UnityWebRequestTexture.GetTexture(_url);

            await request.SendWebRequest().WithCancellation(ct);

            if (request.result != UnityWebRequest.Result.Success)
                throw new WebRequestException($"GET {_url} failed: {request.error}", request.responseCode);

            return DownloadHandlerTexture.GetContent(request);
        }
    }
}
