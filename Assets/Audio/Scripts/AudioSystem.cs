using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class AudioSystem : MonoBehaviour
    {
        public static AudioSystem Instance;

        public Dictionary<AudioType, float> AudioDictionary { get; private set; }
        public AudioData[] MusicTrackArray => _musicTrackArray;

        [Header("REFERENCE")]
        [SerializeField] private AudioData[] _musicTrackArray;

        private AudioSource _musicSource;
        private AudioData _activeTrack;
        private int _currentTrackIndex;

        public void Initialize()
        {
            Instance = this;

            _musicSource = GetComponent<AudioSource>();

            float startVolume = 0.5f;
            AudioDictionary = new()
                {
                    { AudioType.Music, startVolume },
                    { AudioType.Sound, startVolume },
                };

            UpdateMusicTracks(_musicTrackArray);
        }

        public void Tick()
        {
            if (!_musicSource.isPlaying)
            {
                _currentTrackIndex = (_currentTrackIndex + 1) % _musicTrackArray.Length;
                PlayMusic(_musicTrackArray[_currentTrackIndex]);
            }
        }

        public void UpdateMusicTracks(AudioData[] musicArray)
        {
            _currentTrackIndex = 0;
            _musicTrackArray = musicArray;
            PlayMusic(musicArray[_currentTrackIndex]);
        }

        public void UpdateAudioSettings(AudioType type, float volume)
        {
            AudioDictionary[type] = volume;

            if (type == AudioType.Music)
                _musicSource.volume = GetMusicVolume(_activeTrack);
        }

        public void PlayMusic(AudioData data)
        {
            _activeTrack = data;

            _musicSource.Stop();

            _musicSource.clip = data.Clip;
            _musicSource.time = data.Offset;
            _musicSource.volume = GetMusicVolume(data);

            _musicSource.Play();
        }

        public void PlaySound(SoundData data, AudioSource audioSource, bool repeat = true)
        {
            if (audioSource.isPlaying && audioSource.clip == data.Clip && !repeat)
                return;

            audioSource.Stop();

            float[] pitchRanges = data.PitchRanges;
            int index = Random.Range(0, pitchRanges.Length);
            audioSource.pitch = pitchRanges[index];

            audioSource.clip = data.Clip;
            audioSource.time = data.Offset;
            audioSource.volume = GetSoundVolume(data);

            audioSource.Play();
        }

        private float GetMusicVolume(AudioData data) => data.Volume * AudioDictionary[AudioType.Music];

        private float GetSoundVolume(AudioData data) => data.Volume * AudioDictionary[AudioType.Sound];
    }
}