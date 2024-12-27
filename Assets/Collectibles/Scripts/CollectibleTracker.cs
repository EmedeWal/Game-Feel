namespace ShatterStep
{
    public class CollectibleTracker
    {
        public CollectibleType Type;
        public int Current;
        public int Max;

        public CollectibleTracker(CollectibleType type, int current = 0, int max = 0)
        {
            Type = type;
            Current = current;
            Max = max;
        }
    }
}