using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class StatRequirementDisplay : MonoBehaviour
        {
            public void Initialize(Dictionary<StatType, StatValues> statDictionary)
            {
                List<StatObject> statObjectList = new();
                foreach (var obj in GetComponentsInChildren<StatObject>())
                    statObjectList.Add(obj);

                foreach (var type in statDictionary.Keys)
                {
                    int currentValue = statDictionary[type].CurrentValue;
                    int requiredValue = statDictionary[type].MaximumValue;

                    var obj = statObjectList.Find(s => s.Type == type);
                    string count = $"{currentValue} / {requiredValue}";
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = count;
                }
            }
        }
    }
}