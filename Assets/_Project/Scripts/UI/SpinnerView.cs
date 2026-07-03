using DG.Tweening;
using UnityEngine;

namespace TestTask.UI
{
    public sealed class SpinnerView : MonoBehaviour
    {
        [SerializeField] private RectTransform _icon;
        [SerializeField] private float _rotationDuration = 1f;

        private Tween _tween;

        public void Show()
        {
            gameObject.SetActive(true);

            _tween?.Kill();
            _tween = _icon
                .DORotate(new Vector3(0f, 0f, -360f), _rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        public void Hide()
        {
            _tween?.Kill();
            gameObject.SetActive(false);
        }
    }
}
