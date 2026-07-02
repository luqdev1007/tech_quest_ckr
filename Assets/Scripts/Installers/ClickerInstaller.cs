using TestTask.Features.Clicker;
using UnityEngine;
using Zenject;

namespace TestTask.Installers
{
    public sealed class ClickerInstaller : MonoInstaller
    {
        [SerializeField] private ClickerView _clickerView;

        public override void InstallBindings()
        {
            Container.Bind<ClickerView>().FromInstance(_clickerView).AsSingle();

            Container.Bind<CurrencyModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyModel>().AsSingle();

            Container.Bind<ClickerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AutoClicker>().AsSingle();

            Container.BindInterfacesAndSelfTo<ClickerPresenter>().AsSingle();
        }
    }
}
