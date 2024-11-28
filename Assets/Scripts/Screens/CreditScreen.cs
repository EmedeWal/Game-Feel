using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    public class CreditScreen : MonoBehaviour
    {
        private Button _quitGameButton;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _quitGameButton = FindObjectOfType<Button>();

            _quitGameButton.onClick.AddListener(LoadLevel);
        }

        private void OnDisable()
        {
            _quitGameButton.onClick.RemoveListener(LoadLevel);
        }

        private void LoadLevel()
        {
            Application.Quit();
        }
    }
}
