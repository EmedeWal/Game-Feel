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

        [Header("PARENT REFERENCE")]
        [SerializeField] private GameObject _respawnPointParent;

        private List<Trigger> _respawnPointList = new();
        private Vector2 _lastRespawnPosition;

        public void Setup()
        {
            if (_respawnPointParent == null)
            {
                Debug.LogError("Respawn point parent not assigned!");
            }

            _respawnPointList.AddRange(_respawnPointParent.GetComponentsInChildren<Trigger>());
            foreach (Trigger respawnPoint in _respawnPointList)
            {
                respawnPoint.PlayerEntered += RespawnSystem_PlayerEntered;
                respawnPoint.Setup();
            }
        }

        public void Cleanup()
        {
            foreach (Trigger respawnPoint in _respawnPointList)
            {
                respawnPoint.PlayerEntered -= RespawnSystem_PlayerEntered;
            }
        }

        public void RespawnPlayer(Data data)
        {
            data.RespawnPlayer(_lastRespawnPosition);
        }

        private void RespawnSystem_PlayerEntered(Manager player, Trigger respawnPoint)
        {
            _lastRespawnPosition = respawnPoint.transform.position;

            respawnPoint.PlayerEntered -= RespawnSystem_PlayerEntered;
            _respawnPointList.Remove(respawnPoint);
            Destroy(respawnPoint);
        }
    }
}