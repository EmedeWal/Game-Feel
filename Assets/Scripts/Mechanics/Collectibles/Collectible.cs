using UnityEngine;
using System;

namespace Custom
{
    public class Collectible : MonoBehaviour
    {
        public event Action<Collectible> Collected;

        [Header("SETTINGS")]
        public CollectibleType Type;

        public void Init()
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Collected?.Invoke(this);
        }
    }
}