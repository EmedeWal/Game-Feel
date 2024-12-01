using UnityEngine;

namespace ShatterStep
{
    public class AudioSystem : SingletonBase
    {
        #region Setup
        public static AudioSystem Instance; 

        public override void Init()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _musicSource = GetComponent<AudioSource>();

            SetSoundVolume(0.5f);
            SetMusicVolume(0.5f);
        }
        #endregion

        private AudioSource _musicSource;
        private float _soundVolume;

        public void SetSoundVolume(float volume)
        {
            _soundVolume = volume;
        }

        public void SetMusicVolume(float volume)
        {
            _musicSource.volume = volume;
        }

        public float GetSoundVolume()
        {
            return _soundVolume;
        }

        public float GetMusicVolume()
        {
            return _musicSource.volume;
        }

        public void Play(AudioData data, AudioSource audioSource, bool repeat = true)
        {
            if (audioSource.isPlaying && audioSource.clip == data.Clip && !repeat) return;

            audioSource.Stop();

            audioSource.clip = data.Clip;
            audioSource.time = data.Offset;
            audioSource.volume = data.Volume * _soundVolume;

            audioSource.Play();
        }
    }
}