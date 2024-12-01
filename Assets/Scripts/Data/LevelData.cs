using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    [CreateAssetMenu (fileName = "New Level Data", menuName = "Scriptable Object/Data/Level")]
    public class LevelData : ScriptableObject
    {
        [Header("SETTINGS")]
        public string Name = "Level";
        public bool Completed = false;
        public bool Unlocked = false;

        [Header("STATISTICS")]
        public float Time;
        public int Deaths;
        public int Coins;
        public int Keys;

        [Header("COLORS")]
        [Space] public ColorBlock CompletedBlock;
        [Space] public ColorBlock UnlockedBlock;
        [Space] public ColorBlock LockedBlock;
    }
}