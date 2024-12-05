namespace ShatterStep
{
    public static class CollectionHelpers
    {
        public static int GetIndexInBounds(int increment, int index, int length)
        {
            index += increment;

            if (index >= length)
                return 0;

            if (index < 0)
                return length - 1;

            return index;
        }
    }
}