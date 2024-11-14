using UnityEngine;

namespace Custom
{
    namespace Player
    {
        public class InputHandler : MonoBehaviour
        {
            public float HorizontalInput { get; private set; }
            public bool JumpInput { get; private set; }

            private InputActions _inputActions;

            public void Init()
            {
                _inputActions = new();

                _inputActions.Movement.Horizontal.performed += i => HorizontalInput = i.ReadValue<float>();
                _inputActions.Movement.Horizontal.canceled += i => HorizontalInput = i.ReadValue<float>();

                _inputActions.Actions.Jump.performed += i => JumpInput = true;
                _inputActions.Actions.Jump.canceled += i => JumpInput = false;

                _inputActions.Enable();
            }

            public void Cleanup()
            {
                _inputActions.Movement.Horizontal.performed -= i => HorizontalInput = i.ReadValue<float>();
                _inputActions.Movement.Horizontal.canceled -= i => HorizontalInput = i.ReadValue<float>();

                _inputActions.Actions.Jump.performed -= i => JumpInput = true;
                _inputActions.Actions.Jump.canceled -= i => JumpInput = false;

                _inputActions.Disable();
            }
        }
    }
}