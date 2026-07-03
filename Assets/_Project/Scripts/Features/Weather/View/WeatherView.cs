using TestTask.Core.Tabs;
using TestTask.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Features.Weather
{
    public class WeatherView : MonoBehaviour, ITabView
    {
        [SerializeField] private RawImage _iconImage;
        [SerializeField] private TMP_Text _forecastText;

        public TabType Type => TabType.Weather;

        public void SetVisible(bool visible) => gameObject.SetActive(visible);

        public void SetForecast(string text) => _forecastText.text = text;

        public void SetIcon(Texture2D texture)
        {
            _iconImage.texture = texture;
            _iconImage.enabled = true;
        }
    }
}
