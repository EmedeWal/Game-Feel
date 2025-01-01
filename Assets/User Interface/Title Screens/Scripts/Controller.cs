using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class Controller : MonoBehaviour
        {
            [Header("AUDIO REFERENCE")]
            [SerializeField] private SoundData _clickSound;
            private AudioSystem _audioSystem;
            private AudioSource _audioSource;
            private Button[] _buttons;

            private Dictionary<Section, GameObject[]> _sectionDictionary = new();
            private Section _activeSection = null;

            private RectTransform _backgroundRect;
            private float _originalVerticalSize;
            private float _layoutSpacing;

            public void Initialize()
            {
                _audioSystem = AudioSystem.Instance;
                _audioSource = GetComponent<AudioSource>();

                _buttons = FindObjectsOfType<Button>();
                foreach (Button button in _buttons)
                    button.onClick.AddListener(PlayClickAudio);

                Transform mainTransform = transform.GetChild(transform.childCount - 1);
                Section mainSection = mainTransform.GetComponent<Section>();

                _backgroundRect = GetComponent<RectTransform>();
                _originalVerticalSize = _backgroundRect.sizeDelta.y;
                _layoutSpacing = mainTransform.GetComponent<VerticalLayoutGroup>().spacing;

                // Collect all sections in the scene, gather their children and add to dictionary
                List<Transform> allChildrenList = new();
                Section[] _sections = FindObjectsOfType<Section>();
                foreach (Section section in _sections)
                {
                    (Section current, Transform[] children) = section.Initialize(this);
                    List<GameObject> childrenList = new();
                    foreach (Transform child in children)
                    {
                        allChildrenList.Add(child);

                        GameObject childObject = child.gameObject;
                        childrenList.Add(childObject);
                        childObject.SetActive(false);
                    }
                    _sectionDictionary.Add(current, childrenList.ToArray());
                }

                // Parent all children under main transform vertical layout group
                foreach (Transform child in allChildrenList)
                    child.SetParent(mainTransform, false);

                ActivateSection(mainSection);
            }

            public void Cleanup()
            {
                foreach (Section section in _sectionDictionary.Keys)
                    section.Cleanup();

                foreach (Button button in _buttons)
                    button.onClick.RemoveListener(PlayClickAudio);
            }

            public void ActivateSection(Section section)
            {
                // If there is an active section, disable all its children.
                if (_activeSection)
                    foreach (GameObject child in _sectionDictionary[_activeSection])
                        child.SetActive(false);

                // Enable the children of the new section
                GameObject[] children = _sectionDictionary[section];
                foreach (GameObject child in children)
                    child.SetActive(true);

                _activeSection = section;
                SetBackdropSize(children);
            }

            private void SetBackdropSize(GameObject[] children)
            {
                float targetHeight = 0;
                foreach (GameObject element in children)
                {
                    float childLength = element.GetComponent<RectTransform>().sizeDelta.y;
                    targetHeight += childLength + _layoutSpacing;
                }

                targetHeight += _originalVerticalSize;
                Vector2 sizeDelta = _backgroundRect.sizeDelta;
                sizeDelta.y = targetHeight;
                _backgroundRect.sizeDelta = sizeDelta;
            }

            private void PlayClickAudio() => _audioSystem.PlaySound(_clickSound, _audioSource);
        }
    }
}