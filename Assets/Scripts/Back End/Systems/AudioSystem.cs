using UnityEngine;

namespace Custom
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
        }
        #endregion

        [Header("REFERENCES")]
        [SerializeField] private AudioSource _actionSource;
        [SerializeField] private AudioSource _pickupSource;
        [SerializeField] private AudioSource _succesSource;

        public void Play(AudioData data)
        {
            AudioSource audioSource = GetAudioSource(data.Type);

            audioSource.Stop();

            audioSource.clip = data.Clip;
            audioSource.volume = data.Volume;

            audioSource.Play();
        }

        private AudioSource GetAudioSource(AudioType type)
        {
            AudioSource audioSource = null;

            switch (type)
            {
                case AudioType.Action:
                    audioSource = _actionSource;
                    break;

                case AudioType.Pickup:
                    audioSource = _pickupSource;
                    break;

                case AudioType.Succes:
                    audioSource = _succesSource;
                    break;
            }

            return audioSource;
        }
    }
}