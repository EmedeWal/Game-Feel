using UnityEngine;

namespace Custom
{
    namespace Player
    {
        namespace Locomotion
        {
            public class Model
            {
                public void FixedTick(View view, LocomotionState state, float input)
                {
                    view.AnimatorManager.FixedTick(state, input);
                }

                public void PerformJump(View view, Vector2 direction)
                {
                    StopVerticalVelocity(view);

                    view.Rigidbody.AddForce(direction * view.JumpForce, ForceMode2D.Impulse);
                    view.LastGroundedTime = 0f;
                    view.LastHangingTime = 0;
                    view.LastJumpTime = 0f;
                }

                public void PerformAirJump(View view, Vector2 direction)
                {
                    view.CurrentAirJumps++;

                    PerformJump(view, direction);
                }

                public void PerformWallJump(View view)
                {
                    view.DisableInput(view.WallJumpTime);
                    Vector2 wallJumpDirection = Vector2.up;

                    if (view.LastTouchedWall == Direction.Left)
                    {
                        wallJumpDirection += Vector2.right;
                    }
                    else if (view.LastTouchedWall == Direction.Right)
                    {
                        wallJumpDirection += Vector2.left;
                    }

                    wallJumpDirection = wallJumpDirection.normalized;
                    wallJumpDirection *= view.WallJumpMultiplier;
                    PerformJump(view, wallJumpDirection);
                }

                public void StopVerticalVelocity(View view)
                {
                    view.Rigidbody.gravityScale = view.DefaultGravityScale;
                    view.Rigidbody.velocity = new(view.Rigidbody.velocity.x, 0);
                }

                public void CutVerticalVelocity(View view)
                {
                    view.Rigidbody.AddForce((1 - view.JumpCutMultiplier) * view.Rigidbody.velocity.y * Vector2.down, ForceMode2D.Impulse);
                }

                public void TurnCharacter(View view)
                {
                    Vector3 localScale = view.Transform.localScale;
                    localScale.x *= -1;

                    view.Transform.localScale = localScale;
                    view.FacingRight = !view.FacingRight;
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

                public LocomotionState PerformDash(View view)
                {
                    view.DashTimer = view.DashCooldown;
                    view.DisableInput(view.DashDuration);
                    view.PoolManager.ReuseObject(view.DashEffect, view.Transform.position, Quaternion.identity);
                    view.Rigidbody.AddForce(new Vector2(view.FacingRight ? 1 : -1, 0) * view.DashForce, ForceMode2D.Impulse);

                    return LocomotionState.Dashing;
                }

                public float CalculateMovement(View view, LocomotionState state, float input)
                {
                    float targetSpeed = input * view.MovementSpeed;
                    float speeddifference = targetSpeed - view.Rigidbody.velocity.x;
                    float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01) ? view.Acceleration : view.Deceleration;

                    if (state == LocomotionState.Grounded)
                    {
                        return Mathf.Pow(Mathf.Abs(speeddifference) * accelerationRate, view.VelocityPower) * Mathf.Sign(speeddifference);
                    }
                    else
                    {
                        return Mathf.Pow(Mathf.Abs(speeddifference) * accelerationRate * view.AirMultiplier, view.VelocityPower) * Mathf.Sign(speeddifference);
                    }
                }

                public bool CanDash(View view, LocomotionState state)
                {
                    return state != LocomotionState.Dashing && state != LocomotionState.Hanging && view.DashTimer < 0;
                }

                public bool CanAirJump(View view, LocomotionState state)
                {
                    return (view.CurrentAirJumps < view.MaximumAirJumps) && state != LocomotionState.Grounded && state != LocomotionState.Hanging;
                }

                public bool CanTimedJump(float time)
                {
                    return time > 0;
                }

                public bool DirectionOpposite(View view, float direction)
                {
                    return (view.FacingRight && direction < 0) || (!view.FacingRight && direction > 0);
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
                HandlePhysicsChecks(View view)
                {
                    (Vector2 groundCheckOrigin, Vector2 leftWallCheckOrigin, Vector2 rightWallCheckOrigin) =
                    GetPhysicsOrigins(view.BoxCollider, view.Transform, view.WallCheckOffset);

                    bool touchingRightWall = Physics2D.OverlapCircle(rightWallCheckOrigin, view.GrabCheckRadius, view.WallLayer) && view.FacingRight;
                    bool touchingLeftWall = Physics2D.OverlapCircle(leftWallCheckOrigin, view.GrabCheckRadius, view.WallLayer) && !view.FacingRight;
                    bool grounded = Physics2D.OverlapCircle(groundCheckOrigin, view.GroundCheckRadius, view.GroundedLayers);

                    bool hanging = false;
                    if (view.LastGroundedTime < 0)
                    {
                        if (touchingLeftWall)
                        {
                            view.LastTouchedWall = Direction.Left;
                            hanging = true;
                        }
                        else if (touchingRightWall) 
                        {
                            view.LastTouchedWall = Direction.Right;
                            hanging = true;
                        }
                    }

                    if (grounded) view.LastGroundedTime = view.JumpCoyoteTime;
                    if (hanging) view.LastHangingTime = view.JumpCoyoteTime;

                    return (grounded, hanging);
                }
            }
        }
    }
}