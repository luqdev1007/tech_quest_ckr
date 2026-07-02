using TestTask.Configs;
using UnityEngine;
using Zenject;

namespace TestTask.Installers
{
    [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "TestTask/Installers/Configs Installer")]
    public sealed class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller>
    {
        [SerializeField] private ClickerConfig _clickerConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_clickerConfig).AsSingle();
        }
    }
}
