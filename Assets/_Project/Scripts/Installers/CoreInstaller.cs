using TestTask.Core.Audio;
using TestTask.Core.Network;
using TestTask.Core.Tabs;
using UnityEngine;
using Zenject;

namespace TestTask.Installers
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RequestQueue>().AsSingle();
            Container.BindInterfacesAndSelfTo<TabsService>().AsSingle();

            Container.Bind<AudioSource>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("SoundServiceAudioSource")
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<SoundService>().AsSingle();
        }
    }
}
