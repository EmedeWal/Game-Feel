using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    // Class for the level select statistics tab
    public class StatisticsTab : MonoBehaviour
    {
        private Dictionary<StatType, StatUI> _statisticDictionary = new();

        public void Initialize(Dictionary<StatType, StatValues> statisticDictionary)
        {
            StatUI[] counts = GetComponentsInChildren<StatUI>();
            foreach (StatUI count in counts)
            {
                //_statisticDictionary.Add(count.Type, count);
            }

            foreach (StatType type in statisticDictionary.Keys)
            {
                //if (type == StatType.Coin || type == StatType.Key)
                //{
                //    _statisticDictionary[type].UpdateCount(statisticDictionary[type].CurrentValue, statisticDictionary[type].MaximumValue);
                //}
                //else
                //{
                //    _statisticDictionary[type].UpdateCount(statisticDictionary[type].CurrentValue);
                //}
            }
        }
    }
}