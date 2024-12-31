using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

namespace ShatterStep
{
    namespace UI
    {
        public abstract class SliderSettings : MonoBehaviour
        {
            [Header("REFERENCES")]
            [SerializeField] private Sprite _offSprite;
            [SerializeField] private Sprite _onSprite;

            private Slider _slider;
            private Button _button;
            private Image _buttonImage;

            public virtual void Initialize(float initialValue = 0.5f)
            {
                _slider = GetComponentInChildren<Slider>();
                _button = GetComponentInChildren<Button>();
                _buttonImage = _button.GetComponent<Image>();

                _buttonImage.sprite = initialValue > 0 ? _onSprite : _offSprite;
                _slider.value = initialValue;

                _slider.onValueChanged.AddListener(OnSlide);
                _button.onClick.AddListener(OnToggle);
            }

            public virtual void Cleanup()
            {
                _slider.onValueChanged.RemoveListener(OnSlide);
                _button.onClick.RemoveListener(OnToggle);
            }

            protected abstract void ChangeValue(float value);
            protected abstract float GetPreviousValue();
            protected abstract float GetCurrentValue();
            protected abstract float GetDefaultValue();

            private void OnSlide(float value)
            {
                ChangeValue(value);
                _buttonImage.sprite = value > 0 ? _onSprite : _offSprite;
            }

            private void OnToggle()
            {
                float value = GetCurrentValue();
                if (value > 0)
                {
                    ChangeValue(0);
                    _buttonImage.sprite = _offSprite;
                }
                else
                {
                    float previousValue = GetPreviousValue();
                    float defaultValue = GetDefaultValue();
                    float newValue = previousValue > 0.1f
                        ? previousValue
                        : defaultValue;

                    ChangeValue(newValue);
                    _buttonImage.sprite = _onSprite;
                }
                AdjustSliderValueWithoutInvoke(GetCurrentValue());
            }

            private void AdjustSliderValueWithoutInvoke(float value)
            {
                _slider.onValueChanged.RemoveListener(OnSlide);
                _slider.value = value;
                _slider.onValueChanged.AddListener(OnSlide);
            }
        }

    }
}