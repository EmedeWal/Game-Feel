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
        }
        #endregion

        public void Play(AudioData data, AudioSource audioSource)
        {
            audioSource.Stop();

            audioSource.clip = data.Clip;
            audioSource.time = data.Offset;
            audioSource.volume = data.Volume;

            audioSource.Play();
        }
    }
}