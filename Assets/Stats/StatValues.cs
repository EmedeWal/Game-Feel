using UnityEngine;

namespace ShatterStep
{
    [System.Serializable]
    public struct StatValues
    {
        public int CurrentValue;
        public int MaximumValue;
        public bool IsHighScore;

        public StatValues(int currentValue, int maximumValue)
        {
            CurrentValue = currentValue;
            MaximumValue = maximumValue;
            IsHighScore = false;
        }

        public void UpdateValue(int newValue, bool isHigherBetter)
        {
            if (isHigherBetter)
            {
                IsHighScore = newValue > CurrentValue;
                CurrentValue = Mathf.Max(CurrentValue, newValue);
            }
            else
            {
                IsHighScore = newValue < CurrentValue || CurrentValue == 0;
                CurrentValue = Mathf.Min(CurrentValue, newValue);
            }
        }
    }
}