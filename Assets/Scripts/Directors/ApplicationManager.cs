using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class ApplicationManager : MonoBehaviour
    {
        private RespawnSystem _respawnSystem;
        private InputManager _inputManager;
        private TimeSystem _timeSystem;

        private CollectibleManager _collectibleManager;
        private Manager _player;

        private PotionParent _potionParent;
        private SpikeParent _spikeHandler;

        private void Awake()
        {
            // Static
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SingletonBase[] singletons = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
            foreach (var singleton in singletons) singleton.Init();

            _respawnSystem = RespawnSystem.Instance;
            _inputManager = InputManager.Instance;
            _timeSystem = TimeSystem.Instance;

            _collectibleManager = FindObjectOfType<CollectibleManager>();
            _player = FindObjectOfType<Manager>();

            _respawnSystem.Setup();
            _inputManager.Setup();
            _timeSystem.Setup();

            _collectibleManager.Setup();
            _player.Setup();

            // Variable
            _potionParent = FindObjectOfType<PotionParent>();
            _spikeHandler = FindObjectOfType<SpikeParent>();

            _potionParent?.Setup();
            _spikeHandler?.Setup();
        }

        private void OnDisable()
        {
            // Static
            _respawnSystem.Cleanup();
            _inputManager.Cleanup();

            _player.Cleanup();

            // Variable
            _potionParent?.Cleanup();
            _spikeHandler?.Cleanup();
        }

        public void Update()
        {
            // Static
            float deltaTime = Time.deltaTime;

            _player.Tick(deltaTime);

            float unscaledDeltaTime = Time.unscaledDeltaTime;

            _timeSystem.UnscaledTick(unscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            // Static
            float fixedDeltaTime = Time.fixedDeltaTime;

            _player.FixedTick(fixedDeltaTime);
        }
    }
}