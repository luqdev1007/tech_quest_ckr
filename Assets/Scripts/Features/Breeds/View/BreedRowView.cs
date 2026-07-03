using System;
using TestTask.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TestTask.Features.Breeds
{
    public sealed class BreedRowView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private SpinnerView _spinner;

        public IObservable<Unit> Clicked { get; private set; }

        private void Awake()
        {
            Clicked = _button.OnClickAsObservable();
        }

        public void SetData(int number, string name) => _text.text = $"{number} - {name}";

        public void ShowSpinner() => _spinner.Show();

        public void HideSpinner() => _spinner.Hide();

        public sealed class Factory : PlaceholderFactory<BreedRowView>
        {
        }
    }
}
