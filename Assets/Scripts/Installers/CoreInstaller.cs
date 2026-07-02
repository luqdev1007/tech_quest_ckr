using TestTask.Core.Network;
using TestTask.Core.Tabs;
using Zenject;

namespace TestTask.Installers
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RequestQueue>().AsSingle();
            Container.BindInterfacesAndSelfTo<TabsService>().AsSingle();
        }
    }
}
