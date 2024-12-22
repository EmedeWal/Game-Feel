using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class RespawnSystem : SingletonBase
    {
        #region Singleton
        public static RespawnSystem Instance;

        public override void Initialize()
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

        private AudioSystem _audioSystem;

        [Header("AUDIO REFERENCE")]
        [SerializeField] private SoundData _audioData;
        private AudioSource _audioSource;
        private Vector2 _lastRespawnPosition;

        public void Setup()
        {
            _audioSystem = AudioSystem.Instance;

            _audioSource = GetComponent<AudioSource>();
        }

        public void RespawnPlayer(Data data)
        {
            data.RespawnPlayer(_lastRespawnPosition);
        }

        public void RegisterRespawnPoint(RespawnPoint respawnPoint)
        {
            respawnPoint.GetComponentInChildren<ParticleSystem>().Play();
            _audioSystem.PlaySound(_audioData, _audioSource);

            _lastRespawnPosition = respawnPoint.transform.position;
            Destroy(respawnPoint);
        }
    }
}