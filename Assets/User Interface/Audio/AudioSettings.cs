using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class AudioSettings : MonoBehaviour
        {
            private AudioSystem _audioSystem;

            [Header("SETTINGS")]
            [SerializeField] private AudioType _audioType;
            [SerializeField] private Sprite _offSprite;
            [SerializeField] private Sprite _onSprite;

            private Slider _slider;
            private Button _button;
            private Image _buttonImage;

            public void Initialize()
            {
                _audioSystem = AudioSystem.Instance;

                _slider = GetComponentInChildren<Slider>();
                _button = GetComponentInChildren<Button>();
                _buttonImage = _button.GetComponent<Image>();

                _buttonImage.sprite = _audioSystem.AudioDictionary[_audioType] > 0 ? _onSprite : _offSprite;
                _slider.value = _audioSystem.AudioDictionary[_audioType];

                _slider.onValueChanged.AddListener(SetAudioVolume);
                _button.onClick.AddListener(ToggleAudio);
            }

            public void Cleanup()
            {
                _slider.onValueChanged.RemoveListener(SetAudioVolume);
                _button.onClick.RemoveListener(ToggleAudio);
            }

            private void SetAudioVolume(float value)
            {
                _audioSystem.UpdateAudioSettings(_audioType, value);

                _buttonImage.sprite = _audioSystem.AudioDictionary[_audioType] > 0 ? _onSprite : _offSprite;
            }

            private void ToggleAudio()
            {
                if (_audioSystem.AudioDictionary[_audioType] > 0)
                {
                    _audioSystem.UpdateAudioSettings(_audioType, 0);
                    _buttonImage.sprite = _offSprite;
                }
                else
                {
                    _audioSystem.UpdateAudioSettings(_audioType, 0.5f);
                    _buttonImage.sprite = _onSprite;
                }

                _slider.value = _audioSystem.AudioDictionary[_audioType];
            }
        }
    }
}