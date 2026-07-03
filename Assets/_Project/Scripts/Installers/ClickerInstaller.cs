using TestTask.Features.Clicker;
using TestTask.Features.Clicker.Vfx;
using UnityEngine;
using Zenject;

namespace TestTask.Installers
{
    public sealed class ClickerInstaller : MonoInstaller
    {
        [SerializeField] private ClickerView _clickerView;
        [SerializeField] private RectTransform _vfxLayer;
        [SerializeField] private FlyingCurrencyView _flyingCurrencyPrefab;
        [SerializeField] private ClickerBurstView _burstPrefab;
        [SerializeField] private int _flyingCurrencyPoolInitialSize = 5;
        [SerializeField] private int _burstPoolInitialSize = 3;

        public override void InstallBindings()
        {
            Container.Bind<ClickerView>().FromInstance(_clickerView).AsSingle();

            Container.Bind<CurrencyModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyModel>().AsSingle();

            Container.Bind<ClickerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AutoClicker>().AsSingle();

            Container.BindMemoryPool<FlyingCurrencyView, FlyingCurrencyPool>()
                .WithInitialSize(_flyingCurrencyPoolInitialSize)
                .FromComponentInNewPrefab(_flyingCurrencyPrefab)
                .UnderTransform(_vfxLayer);

            Container.BindMemoryPool<ClickerBurstView, ClickerBurstPool>()
                .WithInitialSize(_burstPoolInitialSize)
                .FromComponentInNewPrefab(_burstPrefab)
                .UnderTransform(_vfxLayer);

            Container.BindInterfacesAndSelfTo<ClickerPresenter>().AsSingle();
        }
    }
}
