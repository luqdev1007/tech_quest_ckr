using UnityEngine;

namespace TestTask.Configs
{
    [CreateAssetMenu(fileName = "ClickerConfig", menuName = "TestTask/Configs/Clicker Config")]
    public sealed class ClickerConfig : ScriptableObject
    {
        [SerializeField] private long _currencyPerClick = 1;
        [SerializeField] private int _clickEnergyCost = 1;
        [SerializeField] private float _autoClickInterval = 3f;
        [SerializeField] private int _autoClickEnergyCost = 1;
        [SerializeField] private int _energyMax = 1000;
        [SerializeField] private int _energyRegenAmount = 10;
        [SerializeField] private float _energyRegenInterval = 10f;

        public long CurrencyPerClick => _currencyPerClick;
        public int ClickEnergyCost => _clickEnergyCost;
        public float AutoClickInterval => _autoClickInterval;
        public int AutoClickEnergyCost => _autoClickEnergyCost;
        public int EnergyMax => _energyMax;
        public int EnergyRegenAmount => _energyRegenAmount;
        public float EnergyRegenInterval => _energyRegenInterval;
    }
}
