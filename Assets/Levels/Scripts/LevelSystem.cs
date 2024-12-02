using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class LevelSystem : MonoBehaviour
    {
        #region Singleton
        public static LevelSystem Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                foreach (var levelData in _levelDataList)
                {
                    levelData.Initialize();
                }

                _levelDataList[0].Unlocked = true;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        [Header("DATA LIST")]
        [SerializeField] private List<LevelData> _levelDataList = new();

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