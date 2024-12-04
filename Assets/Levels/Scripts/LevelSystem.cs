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
            if (levelData.Completed)
            {
                CompareAndUpdateStatistics(tracker, levelData);
            }
            else
            {
                InitializeStatistics(tracker, levelData);
            }

            UnlockNextLevel(levelData);

            if (CurrentLevelData != levelData)
                CurrentLevelData = levelData;
        }

        private void CompareAndUpdateStatistics(StatTracker tracker, LevelData levelData)
        {
            foreach (var stat in tracker.StatDictionary)
            {
                StatType type = stat.Key;
                StatValues newValues = stat.Value;
                StatValues oldValues = levelData.StatDictionary[type];

                oldValues.IsHighScore = false;
                levelData.StatDictionary[type] = oldValues;

                switch (type)
                {
                    case StatType.Time:
                    case StatType.Death:
                        if (newValues.CurrentValue < oldValues.CurrentValue)
                        {
                            newValues.IsHighScore = true;
                            levelData.StatDictionary[type] = new(newValues.CurrentValue, oldValues.MaximumValue, true);
                        }
                        break;

                    case StatType.Coin:
                    case StatType.Key:
                        if (newValues.CurrentValue > oldValues.CurrentValue)
                        {
                            levelData.StatDictionary[type] = new(newValues.CurrentValue, oldValues.MaximumValue, true);
                        }
                        break;
                }
            }
        }

        private void InitializeStatistics(StatTracker tracker, LevelData levelData)
        {
            foreach (var stat in tracker.StatDictionary)
            {
                StatType type = stat.Key;
                StatValues newValues = stat.Value;

                if (type != StatType.Key && type != StatType.Coin || newValues.CurrentValue > 0)
                {
                    newValues.IsHighScore = true;
                }

                levelData.StatDictionary[type] = newValues;
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