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

                Initialize(_musicTrackArray);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Instance.Initialize(_musicTrackArray);
                Destroy(gameObject);
            }
        }
        #endregion

        public Dictionary<AudioType, float> AudioDictionary { get; private set; }

        [Header("REFERENCE")]
        [SerializeField] private AudioData[] _musicTrackArray;

        private AudioSource _musicSource;
        private AudioData _activeTrack;
        private int _currentTrackIndex;

        private void Update()
        {
            if (!_musicSource.isPlaying)
            {
                _currentTrackIndex = (_currentTrackIndex + 1) % _musicTrackArray.Length;
                PlayMusic(_musicTrackArray[_currentTrackIndex]);
            }
        }

        public void Initialize(AudioData[] musicArray)
        {
            _currentTrackIndex = 0;
            _musicTrackArray = musicArray;
            PlayMusic(musicArray[_currentTrackIndex]);
        }

        public void UpdateAudioSettings(AudioType type, float volume)
        {
            AudioDictionary[type] = volume;

            if (type == AudioType.Music)
                _musicSource.volume = GetVolume(_activeTrack);
        }

        public void PlayMusic(AudioData data)
        {
            _activeTrack = data;

            _musicSource.Stop();

            _musicSource.clip = data.Clip;
            _musicSource.time = data.Offset;
            _musicSource.volume = GetVolume(data);

            _musicSource.Play();
        }

        public void PlaySound(AudioData data, AudioSource audioSource, bool repeat = true)
        {
            if (audioSource.isPlaying && audioSource.clip == data.Clip && !repeat)
                return;

            audioSource.Stop();

            float minimum = data.DefaultPitch - data.PitchOffset;
            float maximum = data.DefaultPitch + data.PitchOffset;
            audioSource.pitch = Random.Range(minimum, maximum);

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