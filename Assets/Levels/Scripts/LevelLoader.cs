using UnityEngine.SceneManagement;
using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelLoader : PlayerTrigger
    {
        [Header("LEVEL DATA")]
        [SerializeField] private LevelData _levelData;

        protected override void OnPlayerEntered(Manager player)
        {
            UpdateLevelDataWithImprovedStatistics(StatManager.Instance.StatTracker);
            SceneManager.LoadScene(0);
        }

        private void UpdateLevelDataWithImprovedStatistics(StatTracker tracker)
        {
            foreach (var statistic in tracker.StatDictionary)
            {
                StatType type = statistic.Key;
                StatValues newStatistic = statistic.Value;

                if (_levelData.StatDictionary.TryGetValue(type, out StatValues currentStatistic))
                {
                    switch (type)
                    {
                        case StatType.Time:
                        case StatType.Death:
                            if (newStatistic.CurrentValue < currentStatistic.CurrentValue || currentStatistic.CurrentValue == 0)
                            {
                                _levelData.StatDictionary[type] = newStatistic;
                            }
                            break;

                        case StatType.Coin:
                        case StatType.Key:
                            if (newStatistic.CurrentValue > currentStatistic.CurrentValue)
                            {
                                _levelData.StatDictionary[type] = newStatistic;
                            }
                            break;
                    }
                }
                else
                {
                    _levelData.StatDictionary[type] = newStatistic;
                }
            }

            LevelTracker.Instance.LevelCompleted(_levelData);
        }
    }
}
