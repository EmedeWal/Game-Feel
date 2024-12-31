using ShatterStep.UI;
using UnityEngine;

namespace ShatterStep
{
    public class SingletonHolder : MonoBehaviour
    {
        public static SingletonHolder Instance;

        private ConfirmationDialog _confirmationDialog;
        private BrightnessOverlay _brightnessOverlay;
        private AudioSystem _audioSystem;
        private LevelSystem _levelSystem;
        private SceneLoader _sceneLoader;
        private SaveSystem _saveSystem;

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);

                _confirmationDialog = GetComponentInChildren<ConfirmationDialog>();
                _brightnessOverlay = GetComponentInChildren<BrightnessOverlay>();
                _audioSystem = GetComponentInChildren<AudioSystem>();
                _levelSystem = GetComponentInChildren<LevelSystem>();
                _sceneLoader = GetComponentInChildren<SceneLoader>();
                _saveSystem = GetComponentInChildren<SaveSystem>();

                _confirmationDialog.Initialize();
                _brightnessOverlay.Initialize();
                _audioSystem.Initialize();
                _levelSystem.Initialize();
                _sceneLoader.Initialize();
                _saveSystem.Initialize();

                Instance = this;
            }
            else
            {
                Instance.OnAwake();
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            _audioSystem.Tick();
        }

        public void OnAwake()
        {
            _sceneLoader.HandleSceneStart();
        }
    }
}