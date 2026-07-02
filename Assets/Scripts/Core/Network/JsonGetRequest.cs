using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace TestTask.Core.Network
{
    public sealed class JsonGetRequest<T> : IWebRequest<T>
    {
        private readonly string _url;
        private readonly IReadOnlyDictionary<string, string> _headers;

        public JsonGetRequest(string url, IReadOnlyDictionary<string, string> headers = null)
        {
            _url = url;
            _headers = headers;
        }

        public async UniTask<T> Execute(CancellationToken ct)
        {
            using var request = UnityWebRequest.Get(_url);

            if (_headers != null)
            {
                foreach (var header in _headers)
                    request.SetRequestHeader(header.Key, header.Value);
            }

            await request.SendWebRequest().WithCancellation(ct);

            if (request.result != UnityWebRequest.Result.Success)
                throw new WebRequestException($"GET {_url} failed: {request.error}", request.responseCode);

            return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
        }
    }
}
