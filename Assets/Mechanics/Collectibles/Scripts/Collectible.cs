using UnityEngine;
using System;

namespace ShatterStep
{
    public class Collectible : MonoBehaviour
    {
        public event Action<Collectible> Collected;

        [Header("SETTINGS")]
        public AudioData AudioData;
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