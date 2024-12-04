using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ShatterStep
{
    public class StaticStatUI : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private TextMeshProUGUI _keyText;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _deathText;

        public void Initialize(Dictionary<StatType, StatValues> statDictionary)
        {
            foreach (var type in statDictionary.Keys)
            {
                UpdateStatUI(type, statDictionary[type]);
            }
        }

        private void UpdateStatUI(StatType type, StatValues stat)
        {
            TextMeshProUGUI text = GetText(type);

            if (text == null) return;

            switch (type)
            {
                case StatType.Coin:
                case StatType.Key:
                    text.text = $"{stat.CurrentValue} / {stat.MaximumValue}";
                    break;

                case StatType.Time:
                    text.text = UIHelpers.FormatTime(stat.CurrentValue);
                    break;

                default:
                    text.text = stat.CurrentValue.ToString();
                    break;
            }
        }

        private TextMeshProUGUI GetText(StatType type)
        {
            return type switch
            {
                StatType.Key => _keyText,
                StatType.Coin => _coinText,
                StatType.Time => _timeText,
                StatType.Death => _deathText,
                _ => null,
            };
        }
    }
}