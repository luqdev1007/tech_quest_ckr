using System;
using DG.Tweening;
using TestTask.Configs;
using TMPro;
using UnityEngine;
using Zenject;

namespace TestTask.Features.Clicker.Vfx
{
    public sealed class FlyingCurrencyView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _text;

        [Inject] private readonly ClickerVfxConfig _config;

        public void Play(Vector3 worldPos, int amount, Action onComplete)
        {
            _rectTransform.DOKill();

            _rectTransform.position = worldPos;
            _rectTransform.localScale = Vector3.one * _config.FlyingStartScale;
            _canvasGroup.alpha = 1f;
            _text.text = $"+{amount}";

            var horizontalOffset = UnityEngine.Random.Range(-_config.FlyingHorizontalSpread, _config.FlyingHorizontalSpread);
            var targetPos = worldPos + new Vector3(horizontalOffset, _config.FlyingDistance, 0f);

            var sequence = DOTween.Sequence();
            sequence.SetTarget(_rectTransform);
            sequence.Join(_rectTransform.DOMove(targetPos, _config.FlyingDuration).SetEase(_config.FlyingEase));
            sequence.Join(_rectTransform.DOScale(1f, _config.FlyingDuration).SetEase(_config.FlyingEase));
            sequence.Join(_canvasGroup.DOFade(0f, _config.FlyingDuration).SetEase(Ease.InQuad));
            sequence.OnComplete(() => onComplete?.Invoke());
        }
    }
}
