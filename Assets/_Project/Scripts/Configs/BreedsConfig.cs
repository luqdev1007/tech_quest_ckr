using UnityEngine;

namespace TestTask.Configs
{
    [CreateAssetMenu(fileName = "BreedsConfig", menuName = "TestTask/Configs/Breeds Config")]
    public sealed class BreedsConfig : ScriptableObject
    {
        [SerializeField] private string _baseUrl = "https://dogapi.dog/api/v2";
        [SerializeField] private int _breedsToShow = 10;

        public string BaseUrl => _baseUrl;
        public int BreedsToShow => _breedsToShow;
    }
}
