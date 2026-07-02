using System;
using UnityEngine;

namespace TestTask.Features.Clicker.Vfx
{
    public sealed class ClickerBurstView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Action _onComplete;

        public void Play(Vector3 worldPos, Action onComplete)
        {
            _onComplete = onComplete;
            transform.position = worldPos;
            _particleSystem.Play();
        }

        private void OnParticleSystemStopped()
        {
            var callback = _onComplete;
            _onComplete = null;
            callback?.Invoke();
        }
    }
}
