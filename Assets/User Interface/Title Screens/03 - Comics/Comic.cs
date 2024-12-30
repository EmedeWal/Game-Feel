using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public abstract class Comic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
        {
            [Header("DESCRIPTION")]
            [TextArea][SerializeField] protected string _Description;
            protected RequirementsDisplay _RequirementsDisplay;

            [Header("SCENE SETTINGS")]
            [SerializeField] private string[] _sceneArray;

            private UserInterfaceData _userInterfaceData;
            private Button _openButton;

            public void Initialize(UserInterfaceData data)
            {
                _userInterfaceData = data;

                _openButton = GetComponent<Button>();
                
                _openButton.onClick.AddListener(Click); 

                _openButton.GetComponentInChildren<TextMeshProUGUI>().text = gameObject.name;

                if (RequirementsMet())
                {
                    _openButton.colors = _userInterfaceData.CompletedBlock;
                }
                else
                {
                    _openButton.interactable = false;
                    _openButton.colors = _userInterfaceData.LockedBlock;
                }
            }

            public void OnPointerEnter(PointerEventData data)
            {
                if (_openButton.interactable)
                {
                    PointerEnter();
                }
            }

            public void OnPointerExit(PointerEventData data)
            {
                if (_openButton.interactable)
                {
                    _RequirementsDisplay.SetActive(false);
                }
            }

            protected abstract void PointerEnter();
            protected abstract bool RequirementsMet();

            public void Cleanup()
            {
                _openButton.onClick.RemoveListener(Click);
            }

            private void Click()
            {
                SceneLoader.Instance.EnqueueScenes(_sceneArray);
            }
        }
    }
}