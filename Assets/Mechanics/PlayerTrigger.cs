using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class PlayerTrigger : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Cleanup();
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

        protected virtual void Cleanup()
        {

        }

        protected abstract void OnPlayerEntered(Manager player);
    }
}