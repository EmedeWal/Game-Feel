using UnityEngine;

namespace Custom
{
    namespace Player
    {
        namespace Utilities
        {
            public class LocomotionGetters
            {
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

                public (float lastGroundedTime, bool grounded, bool hanging) 
                HandlePhysicsChecks(BoxCollider2D boxCollider, Transform transform, Vector2 wallCheckOffset, 
                LayerMask ignoreLayers, float groundCheckRadius, float grabCheckRadius, float lastGroundedTime, float jumpCoyoteTime)
                {
                    (Vector2 groundCheckOrigin, Vector2 leftWallCheckOrigin, Vector2 rightWallCheckOrigin) = GetPhysicsOrigins(boxCollider, transform, wallCheckOffset);

                    bool grounded = Physics2D.OverlapCircle(groundCheckOrigin, groundCheckRadius, ignoreLayers);
                    bool touchingLeftWall = Physics2D.OverlapCircle(leftWallCheckOrigin, grabCheckRadius, ignoreLayers);
                    bool touchingRightWall = Physics2D.OverlapCircle(rightWallCheckOrigin, grabCheckRadius, ignoreLayers);

                    if (grounded) lastGroundedTime = jumpCoyoteTime;

                    return (lastGroundedTime, lastGroundedTime > 0, (touchingLeftWall || touchingRightWall) && lastGroundedTime < 0);
                }
            }
        }
    }
}