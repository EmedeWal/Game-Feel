using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "New Level Comic Requirement", menuName = "Scriptable Object/Data/Comic Requirement/Collectible")]
        public class CollectibleComicRequirementData : ComicRequirementData
        {
            [Header("COLLECTIBLES REQUIRED")]
            [SerializeField] private int _minimumKeys;
            [SerializeField] private int _minimumCoins;

            // Represents current and required values
            private Dictionary<StatType, int> _requiredStats;
            private Dictionary<StatType, StatValues> _statDictionary;

            public override void Initialize(GameObject displayObject)
            {
                _requiredStats = new()
                {
                    { StatType.Key, _minimumKeys },
                    { StatType.Coin, _minimumCoins },
                };
                _statDictionary = new();
                GetCurrentCollectibles();

                StatRequirementDisplay display = displayObject.GetComponent<StatRequirementDisplay>();
                display.Initialize(_statDictionary);
            }

            public override bool IsRequirementMet()
            {
                // Compare collectibles
                foreach (var values in _statDictionary.Values)
                {
                    if (values.CurrentValue < values.MaximumValue)
                    {
                        Debug.Log("collectible requirement not met.");
                        return false;
                    }
                }
                Debug.Log("collectible requirement met");
                return true;
            }

            private void GetCurrentCollectibles()
            {
                foreach (var data in LevelSystem.Instance.LevelDataList)
                {
                    foreach (var type in data.StatDictionary.Keys)
                    {
                        if (_requiredStats.ContainsKey(type))
                        {
                            if (_statDictionary.ContainsKey(type))
                            {
                                var currentValue = _statDictionary[type].CurrentValue;
                                var maximumValue = _requiredStats[type];

                                var statValues = new StatValues()
                                {
                                    CurrentValue = currentValue + data.StatDictionary[type].CurrentValue,
                                    MaximumValue = maximumValue
                                };
                                _statDictionary[type] = statValues;
                            }
                            else
                            {
                                var currentValue = data.StatDictionary[type].CurrentValue;
                                var maximumValue = _requiredStats[type];

                                var statValues = new StatValues(currentValue, maximumValue);
                                _statDictionary.Add(type, statValues);
                            }
                        }
                    }
                }
            }
        }
    }
}