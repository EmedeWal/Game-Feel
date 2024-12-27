using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class Section : MonoBehaviour
        {
            protected Button _OpenButton;
            protected Button _QuitButton;

            private Controller _controller;
            private Section _parentSection;

            public virtual (Section section, Transform[] children) Initialize(Controller controller, int childrenStart = 2)
            {
                _controller = controller;
                _parentSection = transform.parent.GetComponent<Section>();

                InitializeButtons(GetComponentsInChildren<Button>());

                if (transform.GetChild(1).TryGetComponent(out TextMeshProUGUI text))
                    InitializeText(text);

                // Collect and return children.
                List<Transform> childrenList = new();
                for (int i = childrenStart; i < transform.childCount; i++)
                    childrenList.Add(transform.GetChild(i));

                return (this, childrenList.ToArray());
            }

            public virtual void Cleanup()
            {
                _QuitButton.onClick.RemoveListener(CloseSection);
                _OpenButton.onClick.RemoveListener(OpenSection);
            }

            protected virtual void InitializeButtons(Button[] buttons)
            {
                _OpenButton = GetComponent<Button>();

                _QuitButton = buttons[^1];

                _OpenButton.onClick.AddListener(OpenSection);
                _QuitButton.onClick.AddListener(CloseSection);
            }

            protected virtual void InitializeText(TextMeshProUGUI text)
            {
                text.text = gameObject.name;
            }

            protected virtual void OpenSection()
            {
                _controller.ActivateSection(this);
            }

            protected virtual void CloseSection()
            {
                _controller.ActivateSection(_parentSection);
            }
        }
    }
}