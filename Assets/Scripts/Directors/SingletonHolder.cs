using ShatterStep.UI;
using UnityEngine;

namespace ShatterStep
{
    public class SingletonHolder : MonoBehaviour
    {
        public static bool Initialized;

        private ConfirmationDialog _confirmationDialog;
        private AudioSystem _audioSystem;
        private LevelSystem _levelSystem;
        private SaveSystem _saveSystem;

        private void Awake()
        {
            _confirmationDialog = GetComponentInChildren<ConfirmationDialog>();
            _audioSystem = GetComponentInChildren<AudioSystem>();
            _levelSystem = GetComponentInChildren<LevelSystem>();
            _saveSystem = GetComponentInChildren<SaveSystem>();

            if (!Initialized)
            {
                DontDestroyOnLoad(gameObject);

                _confirmationDialog.Initialize();
                _audioSystem.Initialize();
                _levelSystem.Initialize();
                _saveSystem.Initialize();

                Initialized = true;
            }
            else
            {
                AudioSystem.Instance.UpdateMusicTracks(_audioSystem.MusicTrackArray);

                Destroy(gameObject);
            }
        }

        private void Update()
        {
            _audioSystem.Tick();
        }
    }
}