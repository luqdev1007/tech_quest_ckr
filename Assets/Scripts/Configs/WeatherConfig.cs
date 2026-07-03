using UnityEngine;

namespace TestTask.Configs
{
    [CreateAssetMenu(fileName = "WeatherConfig", menuName = "TestTask/Configs/Weather Config")]
    public sealed class WeatherConfig : ScriptableObject
    {
        [SerializeField] private string _forecastUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        [SerializeField] private string _userAgent = "UnityTestTask (you@example.com)";
        [SerializeField] private float _pollInterval = 5f;

        public string ForecastUrl => _forecastUrl;
        public string UserAgent => _userAgent;
        public float PollInterval => _pollInterval;
    }
}
