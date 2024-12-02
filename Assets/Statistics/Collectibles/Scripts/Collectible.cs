using ShatterStep.Player;
using UnityEngine;
using System;

namespace ShatterStep
{
    public class Collectible : PlayerTrigger
    {
        public event Action<Collectible> Collected;

        [Header("SETTINGS")]
        public AudioData AudioData;
        public StatType Type;

        protected override void OnPlayerEntered(Manager player)
        {
            Collected?.Invoke(this);
        }
    }
}