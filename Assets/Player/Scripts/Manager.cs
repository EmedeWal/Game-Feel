using UnityEngine;

namespace ShatterStep
{
    namespace Player
    {
        public class Manager : MonoBehaviour
        {
            private InputManager _inputManager;

            [Header("SETTINGS")]
            [SerializeField] private Data _data;

            private AnimatorManager _animatorManager;
            private Feedback _feedback;

            public Controller Controller { get; private set; }

            public void Setup()
            {
                _inputManager = InputManager.Instance;

                _animatorManager = new(GetComponentInChildren<Animator>());
                _feedback = new(_data, GetComponentInChildren<SpriteRenderer>());

                _data.Init(gameObject, _inputManager, _animatorManager);

                Controller = new(_data);
            }

            public void Cleanup()
            {
                _feedback.Cleanup();

                Controller.Cleanup();

                _animatorManager = null;
                _feedback = null;
                Controller = null;
            }

            public void Tick(float deltaTime)
            {
                _feedback.Tick(deltaTime);
            }

            public void FixedTick(float fixedDeltaTime)
            {
                float horizontalInput = _inputManager.HorizontalInput;

                Controller.FixedTick(fixedDeltaTime, horizontalInput);
            }
        }
    }
}