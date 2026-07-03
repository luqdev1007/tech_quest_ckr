using UniRx;

namespace TestTask.Core.Tabs
{
    public interface ITabsService
    {
        IReadOnlyReactiveProperty<TabType> Active { get; }

        void SwitchTo(TabType tab);
    }
}
