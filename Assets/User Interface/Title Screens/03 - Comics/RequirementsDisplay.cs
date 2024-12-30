using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class RequirementsDisplay : MonoBehaviour
        {
            private GameObject _holderObject;
            private TextMeshProUGUI _descriptionText;

            public void Initialize()
            {
                _holderObject = transform.GetChild(0).gameObject;
                _descriptionText = _holderObject.GetComponentInChildren<TextMeshProUGUI>();

                SetActive(false);
            }

            public void DisplayRequirementText(string text)
            {
                SetActive(true);

                _descriptionText.text = text;
            }

            public void SetActive(bool active) => _holderObject.SetActive(active);
        }
    }
}