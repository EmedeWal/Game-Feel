using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class OptionsController : MonoBehaviour
        {
            [Header("SOUND REFERENCES")]
            [SerializeField] private SoundData _pauseData;
            [SerializeField] private SoundData _resumeData;
            private AudioSource _audioSource;

            [Header("TOGGLE SETTINGS")]
            [SerializeField] private float _toggleCooldown = 0.5f;
            private float _toggleTimer = 0;

            private InputManager _inputManager;
            private AudioSystem _audioSystem;
            private TimeSystem _timeSystem;

            private GameObject _holderObject;
            private Button _pauseButton;
            private Button _resumeButton;
            private Button _retryLevelButton;
            private Button _mainMenuButton;
            private Button _quitGameButton;
            private Image _background;

            private SliderSettings[] _sliderSettings;

            public void Setup()
            {
                _inputManager = InputManager.Instance;
                _audioSystem = AudioSystem.Instance;
                _timeSystem = TimeSystem.Instance;

                _audioSource = GetComponent<AudioSource>();
                _pauseButton = transform.GetChild(1).GetComponent<Button>();
                _holderObject = transform.GetChild(0).gameObject;
                _background = _holderObject.GetComponent<Image>();

                Transform holderTransform = _holderObject.transform;
                _resumeButton = holderTransform.GetChild(0).GetComponent<Button>();
                _retryLevelButton = holderTransform.GetChild(1).GetComponent<Button>();
                _mainMenuButton = holderTransform.GetChild(2).GetComponent<Button>();
                _quitGameButton = holderTransform.GetChild(3).GetComponent<Button>();

                _resumeButton.onClick.AddListener(Resume);
                _retryLevelButton.onClick.AddListener(RetryLevel);
                _mainMenuButton.onClick.AddListener(MainMenu);
                _quitGameButton.onClick.AddListener(QuitGame);

                EnableHolder(false);

                _sliderSettings = _holderObject.GetComponentsInChildren<SliderSettings>();
                foreach (var sliderSetting in _sliderSettings)
                    sliderSetting.Initialize();

                _inputManager.PauseInputPerformed += OptionsController_PauseInputPerformed;
                _pauseButton.onClick.AddListener(OptionsController_PauseInputPerformed);
            }

            public void Cleanup()
            {
                foreach (var sliderSetting in _sliderSettings)
                    sliderSetting.Cleanup();

                _inputManager.PauseInputPerformed -= OptionsController_PauseInputPerformed;
                _pauseButton.onClick.RemoveListener(OptionsController_PauseInputPerformed);

                _resumeButton.onClick.RemoveListener(Resume);
                _retryLevelButton?.onClick.RemoveListener(RetryLevel);
                _mainMenuButton.onClick.RemoveListener(MainMenu);
                _quitGameButton.onClick.RemoveListener(QuitGame);
            }

            public void UnscaledTick(float unscaledDeltaTime)
            {
                _toggleTimer -= unscaledDeltaTime;
            }

            private void OptionsController_PauseInputPerformed()
            {
                bool enable = !_holderObject.activeSelf;

                if ((!enable && ConfirmationDialog.Instance.Active) || _toggleTimer > 0)
                    return;

                SoundData sound = enable
                    ? _pauseData
                    : _resumeData;
                _audioSystem.PlaySound(sound, _audioSource);

                EnableHolder(enable);

                float timescale = _holderObject.activeSelf ? 0 : 1;
                _timeSystem.SetTimeScale(timescale);
            }

            private void EnableHolder(bool enable)
            {
                _holderObject.SetActive(enable);
                _background.enabled = enable;

                _toggleTimer = _toggleCooldown;
            }

            private void Resume()
            {
                EnableHolder(false);
            }

            private void RetryLevel()
            {
                string message = "Retry this level? Unsaved progress may be lost.";
                static void action() { SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name); }
                ConfirmationDialog.Instance.ShowDialog(message, action);
            }

            private void MainMenu()
            {
                string message = "Quit to the main menu? Unsaved progress may be lost.";
                static void action() { SceneLoader.Instance.LoadFirstScene(); }
                ConfirmationDialog.Instance.ShowDialog(message, action);
            }

            private void QuitGame()
            {
                string message = "Do you want to quit the game? Unsaved progress may be lost.";
                static void action() { Application.Quit(); }
                ConfirmationDialog.Instance.ShowDialog(message, action);
            }
        }
    }
}