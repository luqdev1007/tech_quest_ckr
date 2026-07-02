using TestTask.Core.Tabs;
using TestTask.UI;
using UnityEngine;

namespace TestTask.Features.Clicker
{
    public class ClickerView : MonoBehaviour, ITabView
    {
        public TabType Type => TabType.Clicker;

        public void SetVisible(bool visible) => gameObject.SetActive(visible);
    }
}
