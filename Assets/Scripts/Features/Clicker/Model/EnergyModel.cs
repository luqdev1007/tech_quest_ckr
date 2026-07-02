using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestTask.Configs;
using UniRx;
using Zenject;

namespace TestTask.Features.Clicker
{
    public sealed class EnergyModel : IInitializable, IDisposable
    {
        private readonly ClickerConfig _config;
        private readonly ReactiveProperty<int> _current;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public EnergyModel(ClickerConfig config)
        {
            _config = config;
            _current = new ReactiveProperty<int>(config.EnergyMax);
        }

        public IReadOnlyReactiveProperty<int> Current => _current;

        public bool TrySpend(int amount)
        {
            if (_current.Value < amount)
                return false;

            _current.Value -= amount;
            return true;
        }

        public void Initialize()
        {
            RunRegenLoop(_cts.Token).Forget();
        }

        private async UniTaskVoid RunRegenLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_config.EnergyRegenInterval), cancellationToken: ct);
                _current.Value = Math.Min(_current.Value + _config.EnergyRegenAmount, _config.EnergyMax);
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
