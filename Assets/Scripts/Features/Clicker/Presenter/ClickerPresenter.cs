using System;
using TestTask.Configs;
using TestTask.Core.Audio;
using TestTask.Features.Clicker.Vfx;
using UniRx;
using UnityEngine;
using Zenject;

namespace TestTask.Features.Clicker
{
    public sealed class ClickerPresenter : IInitializable, IDisposable
    {
        private readonly ClickerView _view;
        private readonly ClickerService _clickerService;
        private readonly CurrencyModel _currency;
        private readonly EnergyModel _energy;
        private readonly ClickerConfig _config;
        private readonly ClickerVfxConfig _vfxConfig;
        private readonly ISoundService _soundService;
        private readonly FlyingCurrencyPool _flyingCurrencyPool;
        private readonly ClickerBurstPool _burstPool;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public ClickerPresenter(
            ClickerView view,
            ClickerService clickerService,
            CurrencyModel currency,
            EnergyModel energy,
            ClickerConfig config,
            ClickerVfxConfig vfxConfig,
            ISoundService soundService,
            FlyingCurrencyPool flyingCurrencyPool,
            ClickerBurstPool burstPool)
        {
            _view = view;
            _clickerService = clickerService;
            _currency = currency;
            _energy = energy;
            _config = config;
            _vfxConfig = vfxConfig;
            _soundService = soundService;
            _flyingCurrencyPool = flyingCurrencyPool;
            _burstPool = burstPool;
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

            _clickerService.ClickPerformed
                .Subscribe(_ => PlayFeedback())
                .AddTo(_disposables);
        }

        private void PlayFeedback()
        {
            var spawnPos = _view.VfxSpawnPoint.position;

            _flyingCurrencyPool.Spawn(spawnPos, (int)_config.CurrencyPerClick);
            _burstPool.Spawn(spawnPos);

            var pitch = UnityEngine.Random.Range(_vfxConfig.PitchMin, _vfxConfig.PitchMax);
            _soundService.PlayOneShot(_vfxConfig.ClickClip, _vfxConfig.Volume, pitch);

            _view.PlayClickFeedback(
                _vfxConfig.ButtonPunchStrength,
                _vfxConfig.ButtonPunchDuration,
                _vfxConfig.ButtonPunchVibrato,
                _vfxConfig.ButtonPunchElasticity);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
