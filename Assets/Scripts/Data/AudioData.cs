
using UnityEngine;

namespace ShatterStep
{
    [CreateAssetMenu(fileName = "Audio Data", menuName = "Scriptable Object/Data/Audio")]
    public class AudioData : ScriptableObject
    {
        [Header("SETTINGS")]
        public AudioClip Clip;
        public AudioType Type;
        public float Volume = 0.5f;
        public float Offset = 0;
        public float DefaultPitch = 1f;
        public float PitchOffset = 0;
    }
}