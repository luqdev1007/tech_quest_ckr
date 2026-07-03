using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestTask.Configs;
using Zenject;

namespace TestTask.Features.Clicker
{
    public sealed class AutoClicker : IInitializable, IDisposable
    {
        private readonly ClickerConfig _config;
        private readonly ClickerService _clickerService;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public AutoClicker(ClickerConfig config, ClickerService clickerService)
        {
            _config = config;
            _clickerService = clickerService;
        }

        public void Initialize()
        {
            RunLoop(_cts.Token).Forget();
        }

        private async UniTaskVoid RunLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_config.AutoClickInterval), cancellationToken: ct);
                _clickerService.TryAutoClick();
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
