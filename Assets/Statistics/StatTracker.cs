using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class StatTracker
    {
        public Dictionary<StatType, StatValues> StatDictionary {  get; private set; }

        public delegate void StatModifiedDelegate(StatType type);
        public event StatModifiedDelegate StatModified;

        public StatTracker(Dictionary<StatType, int> maxValues)
        {
            StatDictionary = new Dictionary<StatType, StatValues>();

            foreach (var stat in maxValues)
            {
                StatDictionary[stat.Key] = new StatValues(0, stat.Value);
            }
        }

        public void IncrementStat(StatType type, int increment)
        {
            if (StatDictionary.ContainsKey(type))
            {
                StatValues stat = StatDictionary[type];
                stat.CurrentValue = Mathf.Clamp(stat.CurrentValue + increment, 0, stat.MaximumValue);
                StatDictionary[type] = stat;
                OnStatModified(type);
            }
        }

        private void OnStatModified(StatType type)
        {
            StatModified?.Invoke(type);
        }
    }
}