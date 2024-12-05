using System.Collections.Generic;
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

        [Header("PARENT REFERENCE")]
        [SerializeField] private GameObject _respawnPointParent;

        [Header("AUDIO REFERENCE")]
        [SerializeField] private AudioData _audioData;
        private AudioSource _audioSource;

        private Dictionary<EventTrigger, ParticleSystem> _respawnPointDictionary = new();
        private Vector2 _lastRespawnPosition;

        public void Setup()
        {
            if (_respawnPointParent == null)
            {
                Debug.LogError("Respawn point parent not assigned!");
            }

            _audioSystem = AudioSystem.Instance;

            _audioSource = GetComponent<AudioSource>();

            EventTrigger[] respawnPointArray = _respawnPointParent.GetComponentsInChildren<EventTrigger>();
            foreach (EventTrigger respawnPoint in respawnPointArray)
            {
                ParticleSystem particleSystem = respawnPoint.GetComponent<ParticleSystem>();
                _respawnPointDictionary.Add(respawnPoint, particleSystem);

                respawnPoint.PlayerEntered += RespawnSystem_PlayerEntered;
            }
        }

        public void Cleanup()
        {
            foreach (EventTrigger respawnPoint in _respawnPointDictionary.Keys)
            {
                respawnPoint.PlayerEntered -= RespawnSystem_PlayerEntered;
            }

            _respawnPointDictionary.Clear();
        }

        public void RespawnPlayer(Data data)
        {
            data.RespawnPlayer(_lastRespawnPosition);
        }

        private void RespawnSystem_PlayerEntered(Manager player, EventTrigger respawnPoint)
        {
            _lastRespawnPosition = respawnPoint.transform.position;

            _respawnPointDictionary[respawnPoint].Play();
            _audioSystem.PlaySound(_audioData, _audioSource);

            respawnPoint.PlayerEntered -= RespawnSystem_PlayerEntered;
            _respawnPointDictionary.Remove(respawnPoint);
            Destroy(respawnPoint);
        }
    }
}