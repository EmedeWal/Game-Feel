using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class Play : MonoBehaviour, IElement
        {
            private Button _button;
            private int _level;

            public void Setup(MenuController controller, LevelMenu level)
            {
                _button = GetComponent<Button>();

                //_level = level.LevelIndex;

                _button.onClick.AddListener(OpenPreviousMenu);
            }

            public void Cleanup()
            {
                _button.onClick.RemoveAllListeners();
            }

            private void OpenPreviousMenu()
            {
                SceneManager.LoadScene(_level);
            }
        }
    }
}