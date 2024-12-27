using UnityEngine;

namespace ShatterStep
{
    public class UIHelpers : MonoBehaviour
    {
        public static string FormatTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            return $"{minutes:D2}.{seconds:D2}";
        }
    }
}