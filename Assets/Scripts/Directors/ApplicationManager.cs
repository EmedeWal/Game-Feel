using ShatterStep.Player;
using UnityEngine;
using System;

namespace ShatterStep
{
    public class ApplicationManager : SingletonBase
    {
        #region Singleton
        public static ApplicationManager Instance;

        public override void Init()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        public event Action<GameState> GameStateUpdated; 

        private GameState _gameState = GameState.Gameplay;

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
            if (_gameState == GameState.Gameplay)
            {
                float deltaTime = Time.deltaTime;
                float unscaledDeltaTime = Time.unscaledDeltaTime;

                // Static
                _player.Tick(deltaTime);
                _timeSystem.Tick(unscaledDeltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (_gameState == GameState.Gameplay)
            {
                // Static
                float fixedDeltaTime = Time.fixedDeltaTime;

                _player.FixedTick(fixedDeltaTime);
            }
        }

        public void SetGameState(GameState gameState, float duration)
        {
            _gameState = gameState;
            OnGameStateUpdated(_gameState);
            Invoke(nameof(ResetToDefaultState), duration);
        }

        private void ResetToDefaultState()
        {
            _gameState = GameState.Gameplay;
            OnGameStateUpdated(_gameState);
        }

        private void OnGameStateUpdated(GameState gameState)
        {
            GameStateUpdated?.Invoke(gameState);
        }
    }
}