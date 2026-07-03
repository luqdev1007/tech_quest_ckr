using TestTask.Features.Weather;
using UnityEngine;
using Zenject;

namespace TestTask.Installers
{
    public sealed class WeatherInstaller : MonoInstaller
    {
        [SerializeField] private WeatherView _weatherView;

        public override void InstallBindings()
        {
            Container.BindInstance(_weatherView).AsSingle();

            Container.BindInterfacesAndSelfTo<WeatherPresenter>().AsSingle();
        }
    }
}
