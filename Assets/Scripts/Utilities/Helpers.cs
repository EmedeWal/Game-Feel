using UnityEngine;

namespace Custom
{
    namespace Utilities
    {
        public static class Helpers
        {
            public static Color GetColorBasedOnPercentage(Color[] colors, float max, float value)
            {
                int length = colors.Length;
                float interval = max / length;

                for (int i = 0; i < length; i++)
                {
                    float check = interval * (i + 1);
                    if (value < check) return colors[i];
                }
                return colors[length - 1];
            }
        }
    }
}