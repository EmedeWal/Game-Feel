using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger : MonoBehaviour
    {
        public delegate void PlayerEnteredDelegate(Manager player, Trigger trigger);
        public event PlayerEnteredDelegate PlayerEntered;

        public void Init()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Manager player))
            {
                PlayerEntered?.Invoke(player, this);
            }
        }
    }
}