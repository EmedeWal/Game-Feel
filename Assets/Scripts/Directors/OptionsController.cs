using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    public class OptionsController : MonoBehaviour
    {
        private InputManager _inputManager;
        private TimeSystem _timeSystem;

        private GameObject _holderObject;
        private Button _pauseButton;

        private AudioSettings[] _audioSettingsArray;

        public void Setup()
        {
            _inputManager = InputManager.Instance;
            _timeSystem = TimeSystem.Instance;

            _holderObject = transform.GetChild(0).gameObject;
            _pauseButton = transform.GetChild(1).GetComponent<Button>();

            _holderObject.SetActive(false);

            _audioSettingsArray = _holderObject.GetComponentsInChildren<AudioSettings>();
            foreach (AudioSettings audioSettings in _audioSettingsArray) audioSettings.Setup();
            _inputManager.PauseInputPerformed += OptionsController_PauseInputPerformed;
            _pauseButton.onClick.AddListener(OptionsController_PauseInputPerformed);
        }

        public void Cleanup()
        {
            foreach (AudioSettings audioSettings in _audioSettingsArray) audioSettings.Cleanup();
            _inputManager.PauseInputPerformed -= OptionsController_PauseInputPerformed;
            _pauseButton.onClick.RemoveListener(OptionsController_PauseInputPerformed);
        }

        private void OptionsController_PauseInputPerformed()
        {
            _holderObject.SetActive(!_holderObject.activeSelf);

            float timescale = _holderObject.activeSelf ? 0 : 1;
            _timeSystem.SetTimeScale(timescale);
        }
    }
}