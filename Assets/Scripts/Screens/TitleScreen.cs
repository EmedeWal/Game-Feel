using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    public class TitleScreen : MonoBehaviour
    {
        private Button _startGameButton;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _startGameButton = FindObjectOfType<Button>();

            _startGameButton.onClick.AddListener(LoadLevel);   
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(LoadLevel);
        }

        private void LoadLevel()
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextIndex);
        }
    }
}
