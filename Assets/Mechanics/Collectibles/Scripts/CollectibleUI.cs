using UnityEngine;
using TMPro;

namespace ShatterStep
{
    public class CollectibleUI : MonoBehaviour
    {
        [Header("SETTINGS")]
        public CollectibleType Type;

        private TextMeshProUGUI _text;
        private int _max;
        private int _current;

        public void Setup(int max)
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();

            _max = max;

            _current = 0;   
        }

        public void UpdateCounter(int increment)
        {
            _current += increment;

            _text.text = $"{_current} / {_max}";
        }
    }
}