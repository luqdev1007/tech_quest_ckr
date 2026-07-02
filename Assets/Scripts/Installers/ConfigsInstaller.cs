using TestTask.Configs;
using UnityEngine;
using Zenject;

namespace TestTask.Installers
{
    [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "TestTask/Installers/Configs Installer")]
    public sealed class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller>
    {
        [SerializeField] private ClickerConfig _clickerConfig;
        [SerializeField] private ClickerVfxConfig _clickerVfxConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_clickerConfig).AsSingle();
            Container.BindInstance(_clickerVfxConfig).AsSingle();
        }
    }
}
