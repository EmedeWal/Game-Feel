using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace TitleScreen
    {
        public class Section : MonoBehaviour
        {
            [Header("REFERENCES")]
            [SerializeField] private Button _quitButton;
            private Button _openButton;

            [Header("SETTINGS")]
            [SerializeField] private int _childrenStart = 2;

            private Controller _controller;
            private Section _parentSection;

            public (Section section, Transform[] children) Initialize(Controller controller)
            {
                _controller = controller;
                _parentSection = transform.parent.GetComponent<Section>();

                _openButton = GetComponent<Button>();

                _quitButton.onClick.AddListener(CloseSection);
                _openButton.onClick.AddListener(OpenSection);

                if (transform.GetChild(1).TryGetComponent(out TextMeshProUGUI text))
                    text.text = gameObject.name;

                // Collect and return children.
                List<Transform> childrenList = new();
                for (int i = _childrenStart; i < transform.childCount; i++)
                    childrenList.Add(transform.GetChild(i));

                return (this, childrenList.ToArray());
            }

            public void Cleanup()
            {
                _quitButton.onClick.RemoveListener(CloseSection);
                _openButton.onClick.RemoveListener(OpenSection);
            }

            protected void OpenSection()
            {
                _controller.ActivateSection(this);
            }

            protected void CloseSection()
            {
                _controller.ActivateSection(_parentSection);
            }
        }
    }
}