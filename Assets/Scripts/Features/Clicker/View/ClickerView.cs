using System;
using TestTask.Core.Tabs;
using TestTask.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Features.Clicker
{
    public class ClickerView : MonoBehaviour, ITabView
    {
        [SerializeField] private Button _clickButton;
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private TMP_Text _energyText;

        public TabType Type => TabType.Clicker;

        public IObservable<Unit> ClickRequested { get; private set; }

        private void Awake()
        {
            ClickRequested = _clickButton.OnClickAsObservable();
        }

        public void SetVisible(bool visible) => gameObject.SetActive(visible);

        public void SetCurrency(long amount) => _currencyText.text = amount.ToString();

        public void SetEnergy(int amount) => _energyText.text = amount.ToString();
    }
}
