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
        [Inject(Id = "GoldVfxTarget")] private readonly Transform _goldTarget;

        public void Play(Vector3 worldPos, int amount, Action onComplete)
        {
            _rectTransform.DOKill();

            _rectTransform.position = worldPos;
            _rectTransform.localScale = Vector3.one * _config.FlyingStartScale;
            _canvasGroup.alpha = 1f;
            _text.text = $"+{amount}";

            var horizontalOffset = UnityEngine.Random.Range(-_config.FlyingHorizontalSpread, _config.FlyingHorizontalSpread);
            Vector3 peakPos = worldPos + new Vector3(horizontalOffset, _config.FlyingDistance, 0f);

            Vector3 targetPos = _goldTarget.position;
            Vector3[] pathPoints = new Vector3[] { peakPos, targetPos };

            var sequence = DOTween.Sequence();
            sequence.SetTarget(_rectTransform);

            sequence.Join(_rectTransform.DOPath(pathPoints, _config.FlyingDuration, PathType.CatmullRom)
                .SetEase(_config.FlyingEase));

            sequence.Join(_rectTransform.DOScale(1f, _config.FlyingDuration).SetEase(_config.FlyingEase));
            sequence.Join(_canvasGroup.DOFade(0f, _config.FlyingDuration).SetEase(Ease.InQuad));
            sequence.OnComplete(() => onComplete?.Invoke());
        }
    }
}