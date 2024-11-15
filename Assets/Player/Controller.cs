using UnityEngine;

namespace Custom
{
    namespace Player
    {
        public class Controller : MonoBehaviour
        {
            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;
            private Locomotion _locomotion;

            private void Awake()
            {
                _inputHandler = GetComponent<InputHandler>();
                _animatorManager = GetComponent<AnimatorManager>();
                _locomotion = GetComponent<Locomotion>();

                _inputHandler.Init();
                _animatorManager.Init();
                _locomotion.Init();
            }

            private void OnDisable()
            {
                _locomotion.Cleanup();
                _inputHandler.Cleanup();
            }

            private void FixedUpdate()
            {
                float deltaTime = Time.fixedDeltaTime;
                float horizontalInput = _inputHandler.HorizontalInput;

                _locomotion.FixedTick(deltaTime, horizontalInput);
            }
        }
    }
}