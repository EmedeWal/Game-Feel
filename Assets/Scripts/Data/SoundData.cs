using UnityEngine;

namespace ShatterStep
{
    [CreateAssetMenu(fileName = "Audio Data", menuName = "Scriptable Object/Data/Sound")]
    public class SoundData : AudioData
    {
        public float[] PitchRanges = new float[3];
    }
}