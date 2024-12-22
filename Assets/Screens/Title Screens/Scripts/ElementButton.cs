using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace TitleScreen
    {
        public abstract class ElementButton : MonoBehaviour, IElement
        {
            protected Controller _Controller;
            protected Button _Button;

            public virtual void Initialize(Controller controller)
            {
                _Controller = controller;

                _Button = GetComponent<Button>();

                _Button.onClick.AddListener(OnClick);
            }

            public virtual void Cleanup()
            {
                _Button.onClick.RemoveListener(OnClick);
            }

            protected abstract void OnClick();
        }
    }
}