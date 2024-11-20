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
                [SerializeField] private Data _data;
                private Handler _handler;

                public void Init()
                {
                    _data.Init(gameObject);

                    _data.InputHandler.JumpInputPerformed += Locomotion_JumpInputPerformed;
                    _data.InputHandler.JumpInputCanceled += Locomotion_JumpInputCanceled;
                    _data.InputHandler.DashInputPerformed += Locomotion_DashInputPerformed;

                    _handler = new(_data);
                }

                public void Cleanup()
                {
                    _handler = null;

                    _data.InputHandler.JumpInputPerformed -= Locomotion_JumpInputPerformed;
                    _data.InputHandler.JumpInputCanceled -= Locomotion_JumpInputCanceled;
                    _data.InputHandler.DashInputPerformed -= Locomotion_DashInputPerformed;

                    _data = null;
                }

                public void FixedTick(float deltaTime, float horizontalInput)
                {
                    float inputMagnitude = Mathf.Abs(horizontalInput);
                    UpdateLocomotionState(_data.Rigidbody.velocity.y);

                    HandlePhysics(inputMagnitude);
                    HandleVisuals(inputMagnitude);

                    if (_data.HandleInput)
                    {
                        HandleVerticalMovement();
                        HandleHorizontalMovement(horizontalInput);
                    }

                    _data.FixedTick(deltaTime);
                }

                private void Locomotion_JumpInputPerformed()
                {
                    _data.LastJumpTime = _data.JumpBufferTime;
                }

                private void Locomotion_JumpInputCanceled()
                {
                    if (_data.CurrentLocomotion == LocomotionState.Jumping)
                    {
                        _handler.CutVerticalVelocity();
                    }
                }

                private void Locomotion_DashInputPerformed()
                {
                    if (_handler.CanDash(_data.CurrentLocomotion))
                    {
                        _handler.PerformDash();
                    }
                }

                private void UpdateLocomotionState(float verticalVelocity)
                {
                    (bool grounded, bool hanging) = _handler.HandlePhysicsChecks();
                    if (_data.CurrentLocomotion == LocomotionState.Dashing && !_data.HandleInput) return;
                    (_data.PreviousLocomotion, _data.CurrentLocomotion) = _handler.UpdateLocomotionState(_data.CurrentLocomotion, verticalVelocity, grounded, hanging);
                }

                private void HandleHorizontalMovement(float horizontalInput)
                {
                    float movement = _handler.CalculateMovement(horizontalInput);
                    _data.Rigidbody.AddForce(movement * Vector2.right);
                }

                private void HandleVerticalMovement()
                {
                    if (_data.CurrentLocomotion == LocomotionState.Grounded)
                    {
                        _data.CurrentAirJumps = 0;
                    }

                    if (_data.LastJumpTime > 0)
                    {
                        if (_handler.CanTimedJump(_data.LastHangingTime))
                        {
                            _handler.PerformBounce();
                        }
                        else if (_handler.CanTimedJump(_data.LastGroundedTime))
                        {
                            _handler.PerformJump(LocomotionState.Jumping, Vector2.up);
                        }
                        else if (_handler.CanAirJump(_data.CurrentLocomotion))
                        {
                            _handler.PerformAirJump(Vector2.up * _data.JumpCutMultiplier);
                        }
                    }
                }

                private void HandlePhysics(float inputMagnitude)
                {
                    #region Transitions
                    if (_data.PreviousLocomotion != LocomotionState.Hanging && _data.CurrentLocomotion == LocomotionState.Hanging)
                    {
                        _handler.StopVerticalVelocity(_data.HangingGravityScale);
                    }
                    else if (_data.PreviousLocomotion == LocomotionState.Hanging && _data.CurrentLocomotion != LocomotionState.Hanging)
                    {
                        _data.Rigidbody.gravityScale = _data.DefaultGravityScale;
                    }
                    #endregion

                    #region Gravity
                    if (_data.CurrentLocomotion == LocomotionState.Dashing)
                    {
                        _data.Rigidbody.gravityScale = 0;
                    }
                    else if (_data.CurrentLocomotion == LocomotionState.Grounded)
                    {
                        _data.Rigidbody.gravityScale = _data.DefaultGravityScale;
                    }
                    else if (_data.CurrentLocomotion == LocomotionState.Falling || _data.CurrentLocomotion == LocomotionState.Hanging)
                    {
                        _data.Rigidbody.gravityScale += _data.FallGravityIncrement;
                    }
                    #endregion

                    #region Friction
                    if (_data.CurrentLocomotion == LocomotionState.Grounded && Mathf.Abs(inputMagnitude) < 0.01f)
                    {
                        float amount = Mathf.Min(Mathf.Abs(_data.Rigidbody.velocity.x), Mathf.Abs(_data.FrictionAmount));
                        amount *= Mathf.Sign(_data.Rigidbody.velocity.x);

                        _data.Rigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
                    }

                    if (Mathf.Abs(_data.Rigidbody.velocity.x) < 0.05)
                    {
                        _data.Rigidbody.velocity = new Vector2(0, _data.Rigidbody.velocity.y);
                    }
                    #endregion

                    float newVerticalVelocity = Mathf.Clamp(_data.Rigidbody.velocity.y, -_data.MaximumVerticalVelocity, _data.MaximumVerticalVelocity);
                    _data.Rigidbody.velocity = new(_data.Rigidbody.velocity.x, newVerticalVelocity);
                }

                private void HandleVisuals(float inputMagnitude)
                {
                    _data.DustParticles.rateOverTime = _data.CurrentLocomotion == LocomotionState.Grounded ? _data.Particles : 0;

                    if (_handler.DirectionOpposite(_data.Rigidbody.velocity.x))
                    {
                        _handler.TurnCharacter();
                    }

                    _handler.FixedTick(inputMagnitude);
                }

                private void OnDrawGizmosSelected()
                {
                    if (_data != null && _handler != null)
                    {
                        (Vector2 groundCheckOrigin, Vector2 leftWallCheckOrigin, Vector2 rightWallCheckOrigin) = _handler.GetPhysicsOrigins(_data.BoxCollider, transform, _data.WallCheckOffset);

                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(groundCheckOrigin, _data.GroundCheckRadius);
                        Gizmos.DrawWireSphere(leftWallCheckOrigin, _data.GrabCheckRadius);
                        Gizmos.DrawWireSphere(rightWallCheckOrigin, _data.GrabCheckRadius);
                    }
                }
            }
        }
    }
}