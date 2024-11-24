
using UnityEngine;

namespace Custom
{
    [CreateAssetMenu(fileName = "Audio Data", menuName = "Scriptable Object/Data/Audio")]
    public class AudioData : ScriptableObject
    {
        [Header("SETTINGS")]
        public AudioClip Clip;
        public AudioType Type;
        public float Volume;
    }
}