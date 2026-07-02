using TestTask.Core.Tabs;
using TestTask.UI;
using UnityEngine;

namespace TestTask.Features.Breeds
{
    public class BreedsView : MonoBehaviour, ITabView
    {
        public TabType Type => TabType.Breeds;

        public void SetVisible(bool visible) => gameObject.SetActive(visible);
    }
}
