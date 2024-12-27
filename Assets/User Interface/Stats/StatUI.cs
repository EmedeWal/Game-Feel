using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class StatUI : MonoBehaviour
        {
            [Header("REFERENCES")]
            [SerializeField] private TextMeshProUGUI _keyText;
            [SerializeField] private TextMeshProUGUI _coinText;
            [SerializeField] private TextMeshProUGUI _timeText;
            [SerializeField] private TextMeshProUGUI _deathText;

            private StatTracker _tracker;

            public void Setup(StatTracker tracker)
            {
                _tracker = tracker;

                foreach (var type in _tracker.StatDictionary.Keys)
                {
                    UpdateStatUI(type);
                }

                _tracker.StatModified += UpdateStatUI;
            }

            public void Cleanup()
            {
                _tracker.StatModified -= UpdateStatUI;
            }

            private void UpdateStatUI(StatType type)
            {
                TextMeshProUGUI text = GetText(type);
                StatValues stat = _tracker.StatDictionary[type];

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
}
