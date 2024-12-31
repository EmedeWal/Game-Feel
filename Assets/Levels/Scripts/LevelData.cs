using System.Collections.Generic;
using ShatterStep.UI;
using UnityEngine;

namespace ShatterStep
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "Scriptable Object/Data/Level")]
    public class LevelData : ScriptableObject
    {
        [Header("SETTINGS")]
        public string Name = "Level";
        public bool Completed;
        public bool Unlocked;

        [Header("SCENE QUEUE")]
        public SceneData[] SceneArray;

        [Header("REFERENCE")]
        public UserInterfaceData UI;

        public Dictionary<StatType, StatValues> StatDictionary;

        public void Initialize()
        {
            Completed = false;
            Unlocked = false;

            StatDictionary = new Dictionary<StatType, StatValues>
            {
                { StatType.Key, new StatValues(0, 0) },
                { StatType.Coin, new StatValues(0, 0) },
                { StatType.Time, new StatValues(0, 0) },
                { StatType.Death, new StatValues(0, 0) }
            };
        }
    }
}
