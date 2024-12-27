using System;

namespace ShatterStep
{
    [Serializable]  
    public struct StatValues
    {
        public int CurrentValue;
        public int MaximumValue;
        public bool IsHighScore;

        public StatValues(int currentValue, int maximumValue, bool isHighScore = false)
        {
            CurrentValue = currentValue;
            MaximumValue = maximumValue;
            IsHighScore = isHighScore;
        }
    }
}