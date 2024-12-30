using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        // Used to exit credits
        public class Quit : MonoBehaviour
        {
            private Button _button;

            private void Awake()
            {
                _button = GetComponent<Button>();

                _button.onClick.AddListener(QuitToMainMenu);
            }

            private void OnDisable()
            {
                _button.onClick.RemoveListener(QuitToMainMenu);
            }

            private void QuitToMainMenu()
            {
                SceneLoader.Instance.LoadFirstScene();
            }
        }
    }
}