using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "New Level Comic Requirement", menuName = "Scriptable Object/Data/Comic Requirement/Collectible")]
        public class CollectibleComicRequirementData : ComicRequirementData
        {
            [Header("COLLECTIBLES REQUIRED")]
            [SerializeField] private int _keysRequired;
            [SerializeField] private int _coinsRequired;

            public override bool IsRequirementMet()
            {
                // Do something to check how much keys and coins you have total, and if it meets returns true. Else return false.
                // Also display these amounts using some other scripts not tight to this and initialziing on its own

                return true;
            }
        }
    }
}