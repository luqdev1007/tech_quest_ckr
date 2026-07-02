using TestTask.Core.Tabs;

namespace TestTask.UI
{
    public interface ITabView
    {
        TabType Type { get; }

        void SetVisible(bool visible);
    }
}
