namespace ShatterStep
{
    public struct StatValues 
    {
        public int CurrentValue;
        public int MaximumValue;

        public StatValues(int value, int max)
        {
            CurrentValue = value; MaximumValue = max;
        }
    }
}