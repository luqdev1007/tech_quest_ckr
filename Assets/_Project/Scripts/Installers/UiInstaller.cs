using TestTask.Features.Breeds;
using TestTask.Features.Clicker;
using TestTask.Features.Weather;
using TestTask.UI;
using UnityEngine;
using Zenject;

namespace TestTask.Installers
{
    public class UiInstaller : MonoInstaller
    {
        [SerializeField] private TabBarView _tabBarView;
        [SerializeField] private ClickerView _clickerView;
        [SerializeField] private WeatherView _weatherView;
        [SerializeField] private BreedsView _breedsView;
        [SerializeField] private Transform _goldTextTransform;

        public override void InstallBindings()
        {
            Container.Bind<TabBarView>().FromInstance(_tabBarView).AsSingle();

            Container.Bind<ITabView>().FromInstance(_clickerView).AsCached();
            Container.Bind<ITabView>().FromInstance(_weatherView).AsCached();
            Container.Bind<ITabView>().FromInstance(_breedsView).AsCached();

            Container.BindInterfacesAndSelfTo<TabsPresenter>().AsSingle();

            Container.Bind<Transform>()
            .WithId("GoldVfxTarget")
            .FromInstance(_goldTextTransform);
        }
    }
}
