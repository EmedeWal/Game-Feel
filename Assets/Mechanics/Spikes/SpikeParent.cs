using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class SpikeParent : MonoBehaviour
    {
        private RespawnSystem _respawnSystem;

        private Trigger[] _spikeArray;

        public void Setup()
        {
            _respawnSystem = RespawnSystem.Instance;

            _spikeArray = GetComponentsInChildren<Trigger>();
            foreach (Trigger spike in _spikeArray)
            {
                spike.PlayerEntered += SpikeParent_PlayerEntered;
                spike.Setup();
            }
        }

        public void Cleanup()
        {
            foreach (Trigger spike in _spikeArray)
            {
                spike.PlayerEntered -= SpikeParent_PlayerEntered;
            }
        }

        private void SpikeParent_PlayerEntered(Manager player, Trigger trigger)
        {
            _respawnSystem.RespawnPlayer(player.Controller.Data);
        }
    }
}