using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "New Level Comic Requirement", menuName = "Scriptable Object/Data/Comic Requirement/Level")]
        public class LevelComicRequirementData : ComicRequirementData
        {
            [Header("LEVEL REFERENCE")]
            [SerializeField] private LevelData _levelData;

            public override bool IsRequirementMet() => _levelData.Completed;
        }
    }
}