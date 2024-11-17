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
                    UpdateLocomotionState(_view.Rigidbody.velocity.y);

                    if (_view.HandleInput)
                    {
                        HandleVerticalMovement();
                        HandleHorizontalMovement(horizontalInput);
                    }

                    HandlePhysics(inputMagnitude);
                    HandleVisuals(inputMagnitude);

                    _view.FixedTick(deltaTime);
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
                    if (_model.CanDash(_view, _currentState))
                    {
                        _currentState = _model.PerformDash(_view);
                    }
                }

                private void UpdateLocomotionState(float verticalVelocity)
                {
                    (bool grounded, bool hanging) = 
                    _model.HandlePhysicsChecks(_view);

                    _previousState = _currentState;
                    if (_currentState == LocomotionState.Dashing && !_view.HandleInput) return;
                    _currentState = _model.UpdateLocomotionState(verticalVelocity, grounded, hanging);
                }

                private void HandleHorizontalMovement(float horizontalInput)
                {
                    float movement = _model.CalculateMovement(_view, _currentState, horizontalInput);
                    _view.Rigidbody.AddForce(movement * Vector2.right);
                }

                private void HandleVerticalMovement()
                {
                    if (_currentState == LocomotionState.Grounded)
                    {
                        _view.CurrentAirJumps = 0;
                    }

                    if (_view.LastJumpTime > 0)
                    {
                        if (_model.CanTimedJump(_view.LastHangingTime))
                        {
                            _model.PerformWallJump(_view);
                        }
                        else if (_model.CanTimedJump(_view.LastGroundedTime))
                        {
                            _model.PerformJump(_view, Vector2.up);
                        }
                        else if (_model.CanAirJump(_view, _currentState))
                        {
                            _model.PerformAirJump(_view, Vector2.up);
                        }
                    }
                }

                private void HandlePhysics(float inputMagnitude)
                {
                    #region Transitions
                    if (_previousState != LocomotionState.Hanging && _currentState == LocomotionState.Hanging)
                    {
                        _view.Rigidbody.gravityScale = _view.DefaultGravityScale;
                        _model.CutVerticalVelocity(_view);
                        _view.CurrentAirJumps = 0;
                    }
                    #endregion

                    #region Gravity
                    if (_currentState == LocomotionState.Dashing)
                    {
                        _view.Rigidbody.gravityScale = 0;
                    }
                    else if (_currentState == LocomotionState.Grounded)
                    {
                        _view.Rigidbody.gravityScale = _view.DefaultGravityScale;
                    }
                    else if (_currentState == LocomotionState.Falling || _currentState == LocomotionState.Hanging)
                    {
                        _view.Rigidbody.gravityScale = _view.Rigidbody.gravityScale * _view.FallGravityMultiplier;
                    }
                    #endregion

                    #region Friction
                    if (_currentState == LocomotionState.Grounded && Mathf.Abs(inputMagnitude) < 0.01f)
                    {
                        float amount = Mathf.Min(Mathf.Abs(_view.Rigidbody.velocity.x), Mathf.Abs(_view.FrictionAmount));
                        amount *= Mathf.Sign(_view.Rigidbody.velocity.x);

                        _view.Rigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
                    }
                    else if (Mathf.Abs(_view.Rigidbody.velocity.x) < 0.05)
                    {
                        _view.Rigidbody.velocity = new Vector2(0, _view.Rigidbody.velocity.y);
                    }
                    #endregion

                    float newVerticalVelocity = Mathf.Clamp(_view.Rigidbody.velocity.y, _view.Rigidbody.velocity.y, _view.MaximumVerticalVelocity);
                    _view.Rigidbody.velocity = new(_view.Rigidbody.velocity.x, newVerticalVelocity);
                }

                private void HandleVisuals(float inputMagnitude)
                {
                    if (_model.DirectionOpposite(_view, _view.Rigidbody.velocity.x))
                    {
                        _model.TurnCharacter(_view);
                    }

                    _model.FixedTick(_view, _currentState, inputMagnitude);
                }

                private void OnDrawGizmosSelected()
                {
                    if (_view != null && _model != null)
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