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

                public float CalculateMovement(View view, float horizontalInput)
                {
                    float targetSpeed = horizontalInput * view.MovementSpeed;
                    float speedDifference = targetSpeed - view.Rigidbody.velocity.x;
                    float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01) ? view.Acceleration : view.Deceleration;
                    float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, view.VelocityPower) * Mathf.Sign(speedDifference);

                    return movement;
                }

                public float PerformJump(View view, Vector2 direction)
                {
                    view.Rigidbody.AddForce(direction * view.JumpForce, ForceMode2D.Impulse);

                    return 0f;
                }

                public bool InputDirectionOpposite(View view, float horizontalInput)
                {
                    return (view.FacingRight && horizontalInput < 0) || (!view.FacingRight && horizontalInput > 0);
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

                    bool touchingRightWall = Physics2D.OverlapCircle(rightWallCheckOrigin, view.GrabCheckRadius, view.IgnoreLayers);
                    bool touchingLeftWall = Physics2D.OverlapCircle(leftWallCheckOrigin, view.GrabCheckRadius, view.IgnoreLayers);
                    bool grounded = Physics2D.OverlapCircle(groundCheckOrigin, view.GroundCheckRadius, view.IgnoreLayers);
                    bool hanging = (touchingLeftWall || touchingRightWall) && view.LastGroundedTime < 0;

                    if (grounded) view.LastGroundedTime = view.JumpCoyoteTime;
                    if (hanging) view.LastHangingTime = view.JumpCoyoteTime;

                    return (grounded, hanging);
                }
            }
        }
    }
}