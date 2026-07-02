using System;
using UniRx;
using Zenject;

namespace TestTask.Features.Clicker
{
    public sealed class ClickerPresenter : IInitializable, IDisposable
    {
        private readonly ClickerView _view;
        private readonly ClickerService _clickerService;
        private readonly CurrencyModel _currency;
        private readonly EnergyModel _energy;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public ClickerPresenter(ClickerView view, ClickerService clickerService, CurrencyModel currency, EnergyModel energy)
        {
            _view = view;
            _clickerService = clickerService;
            _currency = currency;
            _energy = energy;
        }

        public void Initialize()
        {
            _currency.Amount
                .Subscribe(_view.SetCurrency)
                .AddTo(_disposables);

            _energy.Current
                .Subscribe(_view.SetEnergy)
                .AddTo(_disposables);

            _view.ClickRequested
                .Subscribe(_ => _clickerService.TryClick())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
