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

        public LevelData CurrentLevelData { get; private set; }

        [Header("DATA LIST")]
        [SerializeField] private List<LevelData> _levelDataList = new();

        public void LevelCompleted(LevelData levelData, StatTracker tracker)
        {
            CompareAndUpdateStatistics(levelData, tracker);
            UnlockNextLevel(levelData);
        }

        private void CompareAndUpdateStatistics(LevelData levelData, StatTracker tracker)
        {
            foreach (var statistic in tracker.StatDictionary)
            {
                StatType type = statistic.Key;
                StatValues newStatistic = statistic.Value;

                if (levelData.StatDictionary.TryGetValue(type, out StatValues currentStatistic))
                {
                    switch (type)
                    {
                        case StatType.Time:
                        case StatType.Death:
                            if (newStatistic.CurrentValue < currentStatistic.CurrentValue || currentStatistic.CurrentValue == 0)
                            {
                                currentStatistic.UpdateValue(newStatistic.CurrentValue, false);
                                levelData.StatDictionary[type] = currentStatistic;
                            }
                            break;

                        case StatType.Coin:
                        case StatType.Key:
                            if (newStatistic.CurrentValue > currentStatistic.CurrentValue)
                            {
                                currentStatistic.UpdateValue(newStatistic.CurrentValue, true);
                                levelData.StatDictionary[type] = currentStatistic;
                            }
                            break;
                    }

                }
                else
                {
                    levelData.StatDictionary[type] = newStatistic;
                }
                CurrentLevelData = levelData;
            }
        }

        private void UnlockNextLevel(LevelData levelData)
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