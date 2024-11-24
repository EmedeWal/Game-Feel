using Custom.Player;
using UnityEngine;

namespace Custom
{
    public class SpikeHandler : MonoBehaviour
    {
        private Trigger[] _triggerArray;

        public void Setup()
        {
            _triggerArray = GetComponentsInChildren<Trigger>();
            foreach (Trigger spike in _triggerArray)
            {
                spike.PlayerEntered += SpikeParent_PlayerEntered;
                spike.Setup();
            }
        }

        public void Cleanup()
        {
            foreach (Trigger spike in _triggerArray)
            {
                spike.PlayerEntered -= SpikeParent_PlayerEntered;
            }
        }

        private void SpikeParent_PlayerEntered(Manager player, Trigger trigger)
        {
            player.Controller.Handler.ResetLastPosition();
        }
    }
}