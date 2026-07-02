using UniRx;

namespace TestTask.Core.Tabs
{
    public sealed class TabsService : ITabsService
    {
        private readonly ReactiveProperty<TabType> _active = new ReactiveProperty<TabType>(TabType.Clicker);

        public IReadOnlyReactiveProperty<TabType> Active => _active;

        public void SwitchTo(TabType tab)
        {
            _active.Value = tab;
        }
    }
}
