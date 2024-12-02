using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class AudioSystem : MonoBehaviour
    {
        #region Singleton
        public static AudioSystem Instance;

        private void Awake()
        {
            _musicSource = GetComponent<AudioSource>();

            if (Instance == null)
            {
                Instance = this;

                AudioDictionary = new()
                {
                    { AudioType.Music, 0.5f },
                    { AudioType.Sound, 0.5f },
                };

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Instance.PlayMusic(_musicSource.clip);
                Destroy(gameObject);
            }
        }
        #endregion

        public Dictionary<AudioType, float> AudioDictionary { get; private set; }

        private AudioSource _musicSource;

        public void UpdateAudioSettings(AudioType type, float volume)
        {
            AudioDictionary[type] = volume;

            if (type == AudioType.Music)
            {
                _musicSource.volume = volume;
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            _musicSource.Stop();

            _musicSource.clip = clip;

            _musicSource.Play();
        }

        public void PlayAudio(AudioData data, AudioSource audioSource, bool repeat = true)
        {
            if (audioSource.isPlaying && audioSource.clip == data.Clip && !repeat) return;

            audioSource.Stop();

            audioSource.clip = data.Clip;
            audioSource.time = data.Offset;
            audioSource.volume = data.Volume * AudioDictionary[AudioType.Sound];

            audioSource.Play();
        }
    }
}