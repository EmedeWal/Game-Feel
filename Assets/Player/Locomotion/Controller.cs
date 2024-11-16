using UnityEngine;

namespace Custom
{
    namespace Player
    {
        namespace Locomotion
        {
            [RequireComponent(typeof(BoxCollider2D))]
            [RequireComponent(typeof(Rigidbody2D))]
            public class Controller : MonoBehaviour
            {
                [Header("DATA REFERENCE")]
                [SerializeField] private View _view;
                private Model _model;

                [SerializeField] private LocomotionState _currentState = LocomotionState.Grounded;
                [SerializeField] private LocomotionState _previousState = LocomotionState.Grounded;

                public void Init()
                {
                    _view.Init(gameObject);

                    _view.InputHandler.JumpInputPerformed += Locomotion_JumpInputPerformed;
                    _view.InputHandler.JumpInputCanceled += Locomotion_JumpInputCanceled;
                    _view.InputHandler.DashInputPerformed += Locomotion_DashInputPerformed;

                    _model = new();
                }

                public void Cleanup()
                {
                    _model = null;

                    _view.InputHandler.JumpInputPerformed -= Locomotion_JumpInputPerformed;
                    _view.InputHandler.JumpInputCanceled -= Locomotion_JumpInputCanceled;
                    _view.InputHandler.DashInputPerformed -= Locomotion_DashInputPerformed;

                    _view = null;
                }

                public void FixedTick(float deltaTime, float horizontalInput)
                {
                    float inputMagnitude = Mathf.Abs(horizontalInput);

                    UpdateLocomotionState(deltaTime, horizontalInput, inputMagnitude, _view.Rigidbody.velocity.y);
                    HandleHorizontalMovement(horizontalInput);
                    HandleVerticalMovement(horizontalInput);
                    HandleFriction(inputMagnitude);
                }

                private void Locomotion_JumpInputPerformed()
                {
                    _view.LastJumpTime = _view.JumpBufferTime;
                }

                private void Locomotion_JumpInputCanceled()
                {
                    if (_currentState == LocomotionState.Jumping)
                    {
                        _model.CutVerticalVelocity(_view);
                    }
                }

                private void Locomotion_DashInputPerformed()
                {

                }

                private void UpdateLocomotionState(float deltaTime, float horizontalInput, float inputMagnitude, float verticalVelocity)
                {
                    #region Updating Fields
                    _view.FixedTick(deltaTime);

                    (bool grounded, bool hanging) = 
                    _model.HandlePhysicsChecks(_view);

                    _previousState = _currentState;
                    _currentState = _model.UpdateLocomotionState(verticalVelocity, grounded, hanging);

                    _model.FixedTick(_view, _currentState, inputMagnitude);
                    #endregion

                    if (_model.InputDirectionOpposite(_view, horizontalInput))
                    {
                        _model.TurnCharacter(_view);
                    }
                }

                private void HandleHorizontalMovement(float horizontalInput)
                {
                    float movement = _model.CalculateMovement(_view, horizontalInput);
                    _view.Rigidbody.AddForce(movement * Vector2.right);
                }

                private void HandleVerticalMovement(float horizontalInput)
                {
                    #region Jumping
                    if (_view.LastJumpTime > 0)
                    {
                        _view.LastJumpTime = 0f;
                        if (_view.LastHangingTime > 0 && _currentState != LocomotionState.Grounded)
                        {
                            _view.LastHangingTime = _model.PerformJump(_view, Vector2.up);
                        }
                        else if (_view.LastGroundedTime > 0)
                        {
                            _view.LastGroundedTime = _view.LastHangingTime = _model.PerformJump(_view, Vector2.up);
                        }
                    }
                    #endregion

                    #region Gravity
                    if (_currentState == LocomotionState.Falling)
                    {
                        _view.Rigidbody.gravityScale = _view.Rigidbody.gravityScale * _view.FallGravityMultiplier;
                    }
                    else
                    {
                        _view.Rigidbody.gravityScale = _view.DefaultGravityScale;
                    }
                    #endregion

                    #region Transitions
                    if (_previousState == LocomotionState.Jumping && _currentState == LocomotionState.Hanging)
                    {
                        _model.CutVerticalVelocity(_view);
                    }
                    #endregion
                }

                private void HandleFriction(float inputMagnitude)
                {
                    #region Drag
                    if (_currentState == LocomotionState.Grounded && inputMagnitude > 0)
                    {
                        _view.Rigidbody.drag = 0;
                    }
                    else
                    {
                        _view.Rigidbody.drag = _view.DragAmount;
                    }
                    #endregion
                }

                private void OnDrawGizmosSelected()
                {
                    if (_view.BoxCollider != null)
                    {
                        (Vector2 groundCheckOrigin, Vector2 leftWallCheckOrigin, Vector2 rightWallCheckOrigin) = _model.GetPhysicsOrigins(_view.BoxCollider, transform, _view.WallCheckOffset);

                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(groundCheckOrigin, _view.GroundCheckRadius);
                        Gizmos.DrawWireSphere(leftWallCheckOrigin, _view.GrabCheckRadius);
                        Gizmos.DrawWireSphere(rightWallCheckOrigin, _view.GrabCheckRadius);
                    }
                }
            }
        }
    }
}