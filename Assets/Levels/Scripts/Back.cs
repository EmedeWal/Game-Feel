using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class Back : MonoBehaviour, IElement
        {
            private MenuController _controller;
            private Button _button;

            public void Setup(MenuController controller, LevelMenu level)
            {
                _controller = controller;

                _button = GetComponent<Button>();

                _button.onClick.AddListener(OpenPreviousMenu);
            }

            public void Cleanup()
            {
                _button.onClick.RemoveAllListeners();
            }

            private void OpenPreviousMenu()
            {
                //_controller.OpenMenu();
            }
        }
    }
}