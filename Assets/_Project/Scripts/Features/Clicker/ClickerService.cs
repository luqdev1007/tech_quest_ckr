using System;
using TestTask.Configs;
using UniRx;

namespace TestTask.Features.Clicker
{
    public sealed class ClickerService
    {
        private readonly ClickerConfig _config;
        private readonly CurrencyModel _currency;
        private readonly EnergyModel _energy;
        private readonly Subject<Unit> _clickPerformed = new Subject<Unit>();

        public ClickerService(ClickerConfig config, CurrencyModel currency, EnergyModel energy)
        {
            _config = config;
            _currency = currency;
            _energy = energy;
        }

        public IObservable<Unit> ClickPerformed => _clickPerformed;

        public bool TryClick() => TryPerform(_config.ClickEnergyCost);

        public bool TryAutoClick() => TryPerform(_config.AutoClickEnergyCost);

        private bool TryPerform(int energyCost)
        {
            if (!_energy.TrySpend(energyCost))
                return false;

            _currency.Add(_config.CurrencyPerClick);
            _clickPerformed.OnNext(Unit.Default);
            return true;
        }
    }
}
