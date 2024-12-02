using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class LevelTracker : MonoBehaviour
    {
        public static LevelTracker Instance;


        [Header("DATA LIST")]
        [SerializeField] private List<LevelData> _levelDataList = new();

        public void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (var levelData in _levelDataList)
            {
                levelData.Initialize();
            }

            _levelDataList[0].Unlocked = true;
        }

        public void LevelCompleted(LevelData levelData)
        {
            int index = _levelDataList.IndexOf(levelData);
            levelData.Completed = true;

            int nextIndex = index + 1; 
            if (nextIndex < _levelDataList.Count)
            {
                _levelDataList[nextIndex].Unlocked = true;
            }
        }
    }
}