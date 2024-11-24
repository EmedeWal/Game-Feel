using Custom.Player;
using UnityEngine;

namespace Custom
{
    public class GameManager : MonoBehaviour
    {
        private TimeSystem _timeSystem;

        private CollectibleManager _collectibleManager;
        private SpikeHandler _spikeHandler;
        private IceHandler _iceHandler;
        private Manager _player;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SingletonBase[] singletons = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
            foreach (var singleton in singletons) singleton.Init();

            _timeSystem = TimeSystem.Instance;

            _collectibleManager = FindObjectOfType<CollectibleManager>();
            _spikeHandler = FindObjectOfType<SpikeHandler>();
            _iceHandler = FindObjectOfType<IceHandler>();
            _player = FindObjectOfType<Manager>();

            _timeSystem.Setup();

            _collectibleManager.Setup();
            _spikeHandler.Setup();
            _iceHandler.Setup();
            _player.Setup();
        }

        private void OnDisable()
        {
            _spikeHandler.Cleanup();
            _iceHandler.Cleanup();
            _player.Cleanup();
        }

        public void Update()
        {
            float deltaTime = Time.deltaTime;

            _iceHandler.Tick(deltaTime);
            _player.Tick(deltaTime);

            float unscaledDeltaTime = Time.unscaledDeltaTime;

            _timeSystem.UnscaledTick(unscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;

            _player.FixedTick(fixedDeltaTime);
        }
    }
}