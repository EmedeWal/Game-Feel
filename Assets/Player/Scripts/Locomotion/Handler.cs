using UnityEngine;

namespace Custom
{
    namespace Player
    {
        namespace Locomotion
        {
            public class Handler
            {
                private Data _d;

                public Handler(Data data) { _d = data; }

                public void FixedTick(LocomotionState state, float input, bool update)
                {
                    _d.AnimatorManager.FixedTick(state, input, update);
                }

                public void PerformJump(Vector2 direction)
                {
                    StopVerticalVelocity();

                    _d.Rigidbody.AddForce(direction * _d.JumpForce, ForceMode2D.Impulse);
                    _d.LastGroundedTime = 0f;
                    _d.LastHangingTime = 0;
                    _d.LastJumpTime = 0f;
                }

                public void PerformAirJump(Vector2 direction)
                {
                    _d.CurrentAirJumps++;

                    PerformJump(direction);
                }

                public void PerformWallJump()
                {
                    _d.DisableInput(_d.WallJumpTime);
                    Vector2 wallJumpDirection = Vector2.up;

                    if (_d.LastTouchedWall == Direction.Left)
                    {
                        wallJumpDirection += Vector2.right;
                    }
                    else if (_d.LastTouchedWall == Direction.Right)
                    {
                        wallJumpDirection += Vector2.left;
                    }

                    wallJumpDirection = wallJumpDirection.normalized;
                    wallJumpDirection *= _d.WallJumpMultiplier;
                    PerformJump(wallJumpDirection);
                }

                public void StopVerticalVelocity()
                {
                    _d.Rigidbody.gravityScale = _d.DefaultGravityScale;
                    _d.Rigidbody.velocity = new(_d.Rigidbody.velocity.x, 0);
                }

                public void CutVerticalVelocity()
                {
                    _d.Rigidbody.AddForce((1 - _d.JumpCutMultiplier) * _d.Rigidbody.velocity.y * Vector2.down, ForceMode2D.Impulse);
                }

                public void TurnCharacter()
                {
                    Vector3 localScale = _d.Transform.localScale;
                    localScale.x *= -1;

                    _d.Transform.localScale = localScale;
                    _d.FacingRight = !_d.FacingRight;
                }

                public LocomotionState UpdateLocomotionState(float verticalVelocity, bool grounded, bool hanging)
                {
                    LocomotionState positionState;

                    if (grounded)
                    {
                        positionState = LocomotionState.Grounded;
                    }
                    else if (hanging)
                    {
                        positionState = LocomotionState.Hanging;
                    }
                    else if (verticalVelocity > 0)
                    {
                        positionState = LocomotionState.Jumping;
                    }
                    else
                    {
                        positionState = LocomotionState.Falling;
                    }

                    return positionState;
                }

                public LocomotionState PerformDash()
                {
                    _d.DashTimer = _d.DashCooldown;
                    _d.DisableInput(_d.DashDuration);
                    _d.PoolManager.ReuseObject(_d.DashEffect, _d.Transform.position, Quaternion.identity);
                    _d.Rigidbody.AddForce(new Vector2(_d.FacingRight ? 1 : -1, 0) * _d.DashForce, ForceMode2D.Impulse);

                    return LocomotionState.Dashing;
                }

                public float CalculateMovement(LocomotionState state, float input)
                {
                    float targetSpeed = input * _d.MovementSpeed;
                    float speeddifference = targetSpeed - _d.Rigidbody.velocity.x;
                    float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01) ? _d.Acceleration : _d.Deceleration;

                    if (state == LocomotionState.Grounded)
                    {
                        return Mathf.Pow(Mathf.Abs(speeddifference) * accelerationRate, _d.VelocityPower) * Mathf.Sign(speeddifference);
                    }
                    else
                    {
                        return Mathf.Pow(Mathf.Abs(speeddifference) * accelerationRate * _d.AirMultiplier, _d.VelocityPower) * Mathf.Sign(speeddifference);
                    }
                }

                public bool CanDash(LocomotionState state)
                {
                    return state != LocomotionState.Dashing && state != LocomotionState.Hanging && _d.DashTimer < 0;
                }

                public bool CanAirJump(LocomotionState state)
                {
                    return (_d.CurrentAirJumps < _d.MaximumAirJumps) && state != LocomotionState.Grounded && state != LocomotionState.Hanging;
                }

                public bool CanTimedJump(float time)
                {
                    return time > 0;
                }

                public bool DirectionOpposite(float direction)
                {
                    return (_d.FacingRight && direction < 0) || (!_d.FacingRight && direction > 0);
                }

                public (Vector2 groundCheckOrigin, Vector2 leftWallCheckOrigin, Vector2 rightWallCheckOrigin)
                GetPhysicsOrigins(BoxCollider2D boxCollider, Transform transform, Vector2 wallCheckOffset)
                {
                    Vector2 groundCheckOrigin = boxCollider.bounds.center;
                    groundCheckOrigin.y -= boxCollider.bounds.extents.y;

                    Vector2 wallCheckOrigin = transform.position;
                    wallCheckOrigin.y += wallCheckOffset.y;

                    Vector2 leftWallCheckOrigin = wallCheckOrigin;
                    leftWallCheckOrigin.x -= wallCheckOffset.x;

                    Vector2 rightWallCheckOrigin = wallCheckOrigin;
                    rightWallCheckOrigin.x += wallCheckOffset.x;

                    return (groundCheckOrigin, leftWallCheckOrigin, rightWallCheckOrigin);
                }

                public (bool grounded, bool hanging)
                HandlePhysicsChecks()
                {
                    (Vector2 groundCheckOrigin, Vector2 leftWallCheckOrigin, Vector2 rightWallCheckOrigin) =
                    GetPhysicsOrigins(_d.BoxCollider, _d.Transform, _d.WallCheckOffset);

                    bool touchingRightWall = Physics2D.OverlapCircle(rightWallCheckOrigin, _d.GrabCheckRadius, _d.WallLayer) && _d.FacingRight;
                    bool touchingLeftWall = Physics2D.OverlapCircle(leftWallCheckOrigin, _d.GrabCheckRadius, _d.WallLayer) && !_d.FacingRight;
                    bool grounded = Physics2D.OverlapCircle(groundCheckOrigin, _d.GroundCheckRadius, _d.GroundedLayers);

                    bool hanging = false;
                    if (_d.LastGroundedTime < 0)
                    {
                        if (touchingLeftWall)
                        {
                            _d.LastTouchedWall = Direction.Left;
                            hanging = true;
                        }
                        else if (touchingRightWall) 
                        {
                            _d.LastTouchedWall = Direction.Right;
                            hanging = true;
                        }
                    }

                    if (grounded) _d.LastGroundedTime = _d.JumpCoyoteTime;
                    if (hanging) _d.LastHangingTime = _d.JumpCoyoteTime;

                    return (grounded, hanging);
                }
            }
        }
    }
}