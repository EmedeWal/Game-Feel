using UnityEngine;

namespace Custom
{
    namespace Player
    {
        public class Controller : MonoBehaviour
        {
            private InputHandler _inputHandler;
            private Locomotion _locomotion;

            private void Awake()
            {
                _inputHandler = GetComponent<InputHandler>();
                _locomotion = GetComponent<Locomotion>();

                _inputHandler.Init();
                _locomotion.Init();
            }

            private void OnDisable()
            {
                _inputHandler.Cleanup();
            }

            private void Update()
            {

            }

            private void LateUpdate()
            {
                
            }

            private void FixedUpdate()
            {
                float deltaTime = Time.fixedDeltaTime;
                float horizontalInput = _inputHandler.HorizontalInput;
                bool jumpInput = _inputHandler.JumpInput;

                _locomotion.FixedTick(deltaTime, horizontalInput, jumpInput);
            }
        }
    }
}