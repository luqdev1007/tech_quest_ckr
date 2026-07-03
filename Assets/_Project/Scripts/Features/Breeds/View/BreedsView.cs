using TestTask.Core.Tabs;
using TestTask.UI;
using UnityEngine;

namespace TestTask.Features.Breeds
{
    public class BreedsView : MonoBehaviour, ITabView
    {
        [SerializeField] private SpinnerView _loader;

        public TabType Type => TabType.Breeds;

        public void SetVisible(bool visible) => gameObject.SetActive(visible);

        public void SetListLoading(bool loading)
        {
            if (loading)
                _loader.Show();
            else
                _loader.Hide();
        }
    }
}
