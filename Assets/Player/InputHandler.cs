using UnityEngine;
using System;

namespace Custom
{
    namespace Player
    {
        public class InputHandler : MonoBehaviour
        {
            public float HorizontalInput { get; private set; }
            public event Action JumpInputPerformed;
            public event Action JumpInputCanceled;
            public event Action DashInputPerformed;

            private InputActions _inputActions;

            public void Init()
            {
                _inputActions = new();

                _inputActions.Movement.Horizontal.performed += i => HorizontalInput = i.ReadValue<float>();
                _inputActions.Movement.Horizontal.canceled += i => HorizontalInput = i.ReadValue<float>();

                _inputActions.Actions.Jump.performed += i => JumpInputPerformed.Invoke();
                _inputActions.Actions.Jump.canceled += i => JumpInputCanceled.Invoke();

                _inputActions.Actions.Dash.performed += i => DashInputPerformed.Invoke();

                _inputActions.Enable();
            }

            public void Cleanup()
            {
                _inputActions.Movement.Horizontal.performed -= i => HorizontalInput = i.ReadValue<float>();
                _inputActions.Movement.Horizontal.canceled -= i => HorizontalInput = i.ReadValue<float>();

                _inputActions.Actions.Jump.performed -= i => JumpInputPerformed.Invoke();
                _inputActions.Actions.Jump.canceled -= i => JumpInputCanceled.Invoke();

                _inputActions.Actions.Dash.performed -= i => DashInputPerformed.Invoke();

                _inputActions.Disable();
            }
        }
    }
}