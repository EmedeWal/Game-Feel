using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class AudioSystem : MonoBehaviour
    {
        public static AudioSystem Instance;

        public Dictionary<AudioType, float> PreviousAudioDictionary { get; private set; }
        public Dictionary<AudioType, float> CurrentAudioDictionary { get; private set; }
        public float DefaultVolume {  get; private set; }   
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

            DefaultVolume = 0.5f;
            CurrentAudioDictionary = new()
            {
                { AudioType.Music, DefaultVolume },
                { AudioType.Sound, DefaultVolume },
            };
            PlayMusic(_musicTrackArray[0], 0);
        }

        public void Tick()
        {
            if (!_musicSource.isPlaying)
            {
                _currentTrackIndex = (_currentTrackIndex + 1) % _musicTrackArray.Length;
                PlayMusic(_musicTrackArray[_currentTrackIndex], _currentTrackIndex);
            }
        }

        public void UpdateMusicTracks(AudioData[] musicArray)
        {
            // Only update if there are different tracks
            if (!AudioArrayIsShared(musicArray))
            {
                _musicTrackArray = musicArray;
                PlayMusic(musicArray[_currentTrackIndex], 0);
            }
        }

        public void SetTypeVolume(AudioType type, float volume)
        {
            // Create a new dictionary and copy the current values
            PreviousAudioDictionary = new Dictionary<AudioType, float>(CurrentAudioDictionary);

            CurrentAudioDictionary[type] = volume;

            if (type == AudioType.Music)
                _musicSource.volume = GetMusicVolume(_activeTrack);
        }

        public void PlayMusic(AudioData data, int trackIndex)
        {
            _currentTrackIndex = trackIndex;

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

        private float GetMusicVolume(AudioData data) => data.Volume * CurrentAudioDictionary[AudioType.Music];

        private float GetSoundVolume(AudioData data) => data.Volume * CurrentAudioDictionary[AudioType.Sound];

        private bool AudioArrayIsShared(AudioData[] musicArray)
        {
            if (musicArray.Length == _musicTrackArray.Length)
            {
                for (int i = 0; i < musicArray.Length; i++)
                {
                    if (_musicTrackArray[i].name != musicArray[i].name)
                    {
                        return false;   
                    }
                }
            }
            return true;
        }
    }
}