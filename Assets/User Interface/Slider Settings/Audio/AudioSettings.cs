using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class AudioSettings : SliderSettings
        {
            private AudioSystem _audioSystem;

            [Header("SETTINGS")]
            [SerializeField] private AudioType _audioType;

            public override void Initialize(float initialValue)
            {
                _audioSystem = AudioSystem.Instance;

                base.Initialize(_audioSystem.CurrentAudioDictionary[_audioType]);
            }

            protected override void ChangeValue(float value) => _audioSystem.SetTypeVolume(_audioType, value);
            protected override float GetPreviousValue() => _audioSystem.PreviousAudioDictionary[_audioType];
            protected override float GetCurrentValue() => _audioSystem.CurrentAudioDictionary[_audioType];
            protected override float GetDefaultValue() => _audioSystem.DefaultVolume;
        }
    }
}