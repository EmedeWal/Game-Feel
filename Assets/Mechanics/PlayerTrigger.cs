using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class PlayerTrigger : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Manager player))
            {
                OnPlayerEntered(player);
            }
        }

        protected virtual void Initialize()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        protected abstract void OnPlayerEntered(Manager player);
    }
}