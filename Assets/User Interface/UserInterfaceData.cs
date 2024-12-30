using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "New UI Data", menuName = "Scriptable Object/Data/UI")]
        public class UserInterfaceData : ScriptableObject
        {
            [Header("COLORS")]
            [Header("Completed Block")]
            [Space] public ColorBlock CompletedBlock;
            [Header("Locked Block")]
            [Space] public ColorBlock LockedBlock;
        }
    }
}