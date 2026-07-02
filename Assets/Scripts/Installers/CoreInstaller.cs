using TestTask.Core.Network;
using Zenject;

namespace TestTask.Installers
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RequestQueue>().AsSingle();
        }
    }
}
