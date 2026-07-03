using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.UI
{
    public sealed class PopupView : MonoBehaviour
    {
        [SerializeField] private RectTransform _root;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Button _okButton;

        public IObservable<Unit> OkClicked => _okButton.OnClickAsObservable();

        public void Show(string title, string description)
        {
            _titleText.text = title;
            _descriptionText.text = description;

            gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_root);
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
