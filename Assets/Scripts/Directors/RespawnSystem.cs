using System.Collections.Generic;
using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class RespawnSystem : SingletonBase
    {
        #region Singleton
        public static RespawnSystem Instance;

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

        private AudioSystem _audioSystem;

        [Header("PARENT REFERENCE")]
        [SerializeField] private GameObject _respawnPointParent;

        [Header("AUDIO REFERENCE")]
        [SerializeField] private AudioData _audioData;
        private AudioSource _audioSource;

        private Dictionary<Trigger, ParticleSystem> _respawnPointDictionary = new();
        private Vector2 _lastRespawnPosition;

        public void Setup()
        {
            if (_respawnPointParent == null)
            {
                Debug.LogError("Respawn point parent not assigned!");
            }

            _audioSystem = AudioSystem.Instance;

            _audioSource = GetComponent<AudioSource>();

            Trigger[] respawnPointArray = _respawnPointParent.GetComponentsInChildren<Trigger>();
            foreach (Trigger respawnPoint in respawnPointArray)
            {
                ParticleSystem particleSystem = respawnPoint.GetComponent<ParticleSystem>();
                _respawnPointDictionary.Add(respawnPoint, particleSystem);

                respawnPoint.PlayerEntered += RespawnSystem_PlayerEntered;
                respawnPoint.Init();
            }
        }

        public void Cleanup()
        {
            foreach (Trigger respawnPoint in _respawnPointDictionary.Keys)
            {
                respawnPoint.PlayerEntered -= RespawnSystem_PlayerEntered;
            }

            _respawnPointDictionary.Clear();
        }

        public void RespawnPlayer(Data data)
        {
            data.RespawnPlayer(_lastRespawnPosition);
        }

        private void RespawnSystem_PlayerEntered(Manager player, Trigger respawnPoint)
        {
            _lastRespawnPosition = respawnPoint.transform.position;

            _respawnPointDictionary[respawnPoint].Play();
            _audioSystem.Play(_audioData, _audioSource);

            respawnPoint.PlayerEntered -= RespawnSystem_PlayerEntered;
            _respawnPointDictionary.Remove(respawnPoint);
            Destroy(respawnPoint);
        }
    }
}