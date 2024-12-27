using ShatterStep.Player;
using ShatterStep.UI;
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

        private OptionsController _optionsController;
        private SnowController _snowController;
        private StatManager _statManager;
        private Manager _player;

        private void Awake()
        {
            foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
                canvas.enabled = true;

            SingletonBase[] singletonArray = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
            foreach (var singleton in singletonArray) singleton.Initialize();

            _respawnSystem = RespawnSystem.Instance;
            _inputManager = InputManager.Instance;
            _timeSystem = TimeSystem.Instance;

            _optionsController = FindObjectOfType<OptionsController>();
            _snowController = FindObjectOfType<SnowController>();
            _statManager = FindObjectOfType<StatManager>();
            _player = FindObjectOfType<Manager>();
        }

        private void Start()
        {
            _respawnSystem.Setup();
            _inputManager.Setup();
            _timeSystem.Setup();

            _optionsController.Setup();
            _statManager.Setup(_player);
            _player.Setup();

            _snowController.Initialize(_player);
        }

        private void OnDisable()
        {
            _timeSystem.Cleanup();

            _inputManager.Cleanup();

            _optionsController.Cleanup();
            _statManager.Cleanup();
            _player.Cleanup();
        }

        private void Update()
        {
            if (_gameState == GameState.Gameplay)
            {
                float deltaTime = Time.deltaTime;
                float unscaledDeltaTime = Time.unscaledDeltaTime;

                _player.Tick(deltaTime);
                _timeSystem.UnscaledTick(unscaledDeltaTime);
                _optionsController.UnscaledTick(unscaledDeltaTime);
            }
        }

        private void LateUpdate()
        {
            if (_gameState == GameState.Gameplay)
            {
                _snowController.LateTick();
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