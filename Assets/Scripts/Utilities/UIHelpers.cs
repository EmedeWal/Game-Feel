using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class UIHelpers : MonoBehaviour
    {
        public static float GetLengthOfElements(List<GameObject> elementList, float layoutSpacing)
        {
            float totalLength = 0;
            foreach (GameObject element in elementList)
            {
                float elementLength = element.GetComponent<RectTransform>().sizeDelta.y;
                totalLength += elementLength + layoutSpacing;
            }
            return totalLength;
        }

        public static float GetLengthOfChildren(Transform transform, float layoutSpacing)
        {
            float totalLength = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                float childLength = transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
                totalLength += childLength + layoutSpacing;
            }
            return totalLength;
        }
    }

}