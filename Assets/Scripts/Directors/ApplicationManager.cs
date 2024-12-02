using ShatterStep.Player;
using UnityEngine;
using System;

namespace ShatterStep
{
    public class ApplicationManager : SingletonBase
    {
        #region Singleton
        public static ApplicationManager Instance;

        public override void Initialize()
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

        private StatManager _statisticsManager;
        private Manager _player;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SingletonBase[] singletons = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
            foreach (var singleton in singletons) singleton.Initialize();

            _respawnSystem = RespawnSystem.Instance;
            _inputManager = InputManager.Instance;
            _timeSystem = TimeSystem.Instance;

            _statisticsManager = FindObjectOfType<StatManager>();
            _player = FindObjectOfType<Manager>();

            _respawnSystem.Setup();
            _inputManager.Setup();
            _timeSystem.Setup();

            _statisticsManager.Setup(_player);
            _player.Setup();
        }

        private void OnDisable()
        {
            _respawnSystem.Cleanup();
            _inputManager.Cleanup();

            _statisticsManager.Cleanup();
            _player.Cleanup();
        }

        public void Update()
        {
            if (_gameState == GameState.Gameplay)
            {
                float deltaTime = Time.deltaTime;
                float unscaledDeltaTime = Time.unscaledDeltaTime;

                _player.Tick(deltaTime);
                _timeSystem.Tick(unscaledDeltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (_gameState == GameState.Gameplay)
            {
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