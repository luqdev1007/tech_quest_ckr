using TestTask.Core.Tabs;
using TestTask.UI;
using UnityEngine;

namespace TestTask.Features.Weather
{
    public class WeatherView : MonoBehaviour, ITabView
    {
        public TabType Type => TabType.Weather;

        public void SetVisible(bool visible) => gameObject.SetActive(visible);
    }
}
