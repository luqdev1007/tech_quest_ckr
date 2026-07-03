using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace TestTask.Core.Network
{
    public sealed class TextureGetRequest : IWebRequest<Texture2D>
    {
        private readonly string _url;
        private readonly IReadOnlyDictionary<string, string> _headers;

        public TextureGetRequest(string url, IReadOnlyDictionary<string, string> headers = null)
        {
            _url = url;
            _headers = headers;
        }

        public async UniTask<Texture2D> Execute(CancellationToken ct)
        {
            using var request = UnityWebRequestTexture.GetTexture(_url);

            if (_headers != null)
            {
                foreach (var header in _headers)
                    request.SetRequestHeader(header.Key, header.Value);
            }

            await request.SendWebRequest().WithCancellation(ct);

            if (request.result != UnityWebRequest.Result.Success)
                throw new WebRequestException($"GET {_url} failed: {request.error}", request.responseCode);

            return DownloadHandlerTexture.GetContent(request);
        }
    }
}
