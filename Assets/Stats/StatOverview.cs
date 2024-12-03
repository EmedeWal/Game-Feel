using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    public class StatOverview : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private Sprite[] _iconArray;

        private TextMeshProUGUI _highScoreText;
        private TextMeshProUGUI _countText;
        private Image _icon;

        public void Initialize(StatType type, StatValues values)
        {
            _highScoreText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _countText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            _icon = transform.GetChild(3).GetComponent<Image>();

            UpdateText(type, values);
            UpdateIcon(type);
        }

        private void UpdateText(StatType type, StatValues values)
        {
            _highScoreText.text = values.IsHighScore ? " NEW HIGHSCORE!" : "";

            switch (type)
            {
                case StatType.Coin:
                case StatType.Key:
                    _countText.text = $"{values.CurrentValue} / {values.MaximumValue}";
                    break;

                case StatType.Time:
                    _countText.text = UIHelpers.FormatTime(values.CurrentValue);
                    break;

                default:
                    _countText.text = values.CurrentValue.ToString();
                    break;
            }
        }

        private void UpdateIcon(StatType type)
        {
            switch (type)
            {
                case StatType.Key:
                    _icon.sprite = _iconArray[0];
                    break;

                case StatType.Coin:
                    _icon.sprite = _iconArray[1];
                    break;

                case StatType.Time:
                    _icon.sprite = _iconArray[2];
                    break;

                case StatType.Death:
                    _icon.sprite = _iconArray[3];
                    break;
            }
        }
    }
}
