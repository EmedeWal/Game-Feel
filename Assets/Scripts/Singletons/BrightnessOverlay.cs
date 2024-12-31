using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    public class BrightnessOverlay : MonoBehaviour
    {
        [Header("SETTINGS")]
        [SerializeField] private float _alphaMultiplier = 0.3f;

        public static BrightnessOverlay Instance { get; private set; }
        public float PreviousBrightness { get; private set; }
        public float CurrentBrightness { get; private set; }
        public float DefaultBrightness { get; private set; }

        private Image _overlay;

        public void Initialize()
        {
            Instance = this;

            DefaultBrightness = 1f;

            _overlay = GetComponentInChildren<Image>();
            CurrentBrightness = DefaultBrightness;

            SetBrightness(DefaultBrightness);
        }

        public void SetBrightness(float brightness)
        {
            PreviousBrightness = CurrentBrightness;

            CurrentBrightness = brightness;

            float alpha = 1 - brightness;
            alpha *= _alphaMultiplier;

            SetAlpha(alpha);
        }

        private void SetAlpha(float alpha)
        {
            Color color = _overlay.color;
            color.a = alpha;
            _overlay.color = color;
        }
    }
}