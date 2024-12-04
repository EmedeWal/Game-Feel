using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

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

                PlayMusic(MusicData);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (Instance.MusicData != MusicData)
                    Instance.PlayMusic(MusicData);

                Destroy(gameObject);
            }
        }
        #endregion

        [Header("REFERENCE")]
        public AudioData MusicData;

        public Dictionary<AudioType, float> AudioDictionary { get; private set; }

        private AudioSource _musicSource;

        public void UpdateAudioSettings(AudioType type, float volume)
        {
            AudioDictionary[type] = volume;

            if (type == AudioType.Music)
                _musicSource.volume = GetVolume(MusicData);
        }

        public void PlayMusic(AudioData data)
        {
            MusicData = data;

            _musicSource.Stop();

            _musicSource.clip = data.Clip;
            _musicSource.time = data.Offset;
            _musicSource.volume = GetVolume(data);

            _musicSource.Play();
        }

        public void Play(AudioData data, AudioSource audioSource, bool repeat = true)
        {
            if (audioSource.isPlaying && audioSource.clip == data.Clip && !repeat) 
                return;

            audioSource.Stop();

            audioSource.clip = data.Clip;
            audioSource.time = data.Offset;
            audioSource.volume = GetVolume(data);

            audioSource.Play();
        }

        private float GetVolume(AudioData data)
        {
            float multiplier = data.Type == AudioType.Music ?
            AudioDictionary[AudioType.Music] : AudioDictionary[AudioType.Sound];

            return data.Volume * multiplier;
        }
    }
}