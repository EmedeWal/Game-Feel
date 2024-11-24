using UnityEngine;

namespace Custom
{
    namespace Player
    {
        public class Manager : MonoBehaviour
        {
            [Header("SETTINGS")]
            [SerializeField] private Data _data;

            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;

            public Controller Controller { get; private set; }

            private Feedback _feedback;

            public void Setup()
            {
                _inputHandler = new();
                _animatorManager = new(GetComponentInChildren<Animator>());
                Controller = new(_data, gameObject, _inputHandler, _animatorManager);
                _feedback = new(Controller, GetComponentInChildren<SpriteRenderer>(), Color.blue);
            }

            public void Cleanup()
            {
                Controller.Cleanup();
                _inputHandler.Cleanup();

                _feedback = null;
                Controller = null;
                _animatorManager = null;
                _inputHandler = null;
            }

            public void Tick(float deltaTime)
            {
                _feedback.Tick(deltaTime);
            }

            public void FixedTick(float fixedDeltaTime)
            {
                float horizontalInput = _inputHandler.HorizontalInput;

                Controller.FixedTick(fixedDeltaTime, horizontalInput);
            }
        }
    }
}