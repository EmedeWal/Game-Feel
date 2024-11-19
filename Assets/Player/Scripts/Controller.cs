using UnityEngine;

namespace Custom
{
    namespace Player
    {
        public class Controller : MonoBehaviour
        {
            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;
            private Locomotion.Controller _locomotion;

            public void Init()
            {
                _inputHandler = GetComponent<InputHandler>();
                _animatorManager = GetComponent<AnimatorManager>();
                _locomotion = GetComponent<Locomotion.Controller>();

                _inputHandler.Init();
                _animatorManager.Init();
                _locomotion.Init();
            }

            public void Cleanup()
            {
                _locomotion.Cleanup();
                _inputHandler.Cleanup();
            }

            public void FixedTick(float fixedDeltaTime)
            {
                float horizontalInput = _inputHandler.HorizontalInput;

                _locomotion.FixedTick(fixedDeltaTime, horizontalInput);
            }
        }
    }
}