using DG.Tweening;
using UnityEngine;

namespace TestTask.Configs
{
    [CreateAssetMenu(fileName = "ClickerVfxConfig", menuName = "TestTask/Configs/Clicker Vfx Config")]
    public sealed class ClickerVfxConfig : ScriptableObject
    {
        [Header("Flying Currency")]
        [SerializeField] private float _flyingDistance = 150f;
        [SerializeField] private float _flyingDuration = 0.6f;
        [SerializeField] private Ease _flyingEase = Ease.OutQuad;
        [SerializeField] private float _flyingHorizontalSpread = 40f;
        [SerializeField] private float _flyingStartScale = 0.6f;

        [Header("Button Punch")]
        [SerializeField] private Vector3 _buttonPunchStrength = new Vector3(0.15f, 0.15f, 0f);
        [SerializeField] private float _buttonPunchDuration = 0.25f;
        [SerializeField] private int _buttonPunchVibrato = 8;
        [SerializeField] private float _buttonPunchElasticity = 0.8f;

        [Header("Sound")]
        [SerializeField] private AudioClip _clickClip;
        [SerializeField] private float _volume = 1f;
        [SerializeField] private float _pitchMin = 0.95f;
        [SerializeField] private float _pitchMax = 1.05f;

        public float FlyingDistance => _flyingDistance;
        public float FlyingDuration => _flyingDuration;
        public Ease FlyingEase => _flyingEase;
        public float FlyingHorizontalSpread => _flyingHorizontalSpread;
        public float FlyingStartScale => _flyingStartScale;

        public Vector3 ButtonPunchStrength => _buttonPunchStrength;
        public float ButtonPunchDuration => _buttonPunchDuration;
        public int ButtonPunchVibrato => _buttonPunchVibrato;
        public float ButtonPunchElasticity => _buttonPunchElasticity;

        public AudioClip ClickClip => _clickClip;
        public float Volume => _volume;
        public float PitchMin => _pitchMin;
        public float PitchMax => _pitchMax;
    }
}
