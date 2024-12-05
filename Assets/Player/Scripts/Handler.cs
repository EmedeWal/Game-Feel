using UnityEngine;

namespace ShatterStep
{
    namespace Player
    {
        public class Handler
        {
            private Data _d;

            public Handler(Data data) { _d = data; }

            public void FixedTick(float input)
            {
                _d.AnimatorManager.FixedTick(_d.CurrentLocomotion, input);
            }

            public void PerformSpawn(BoxCollider2D boxCollider)
            {
                Vector2 basePosition = boxCollider.bounds.center;
                Quaternion rotation;
                float xExtends = boxCollider.bounds.extents.x + 0.5f;

                if (_d.FacingRight)
                {
                    basePosition.x += xExtends;
                    rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    basePosition.x -= xExtends;
                    rotation = Quaternion.Euler(0, 0, 270);
                }

                (Vector2 position, bool allowed) = FindNearestValidPosition(basePosition, rotation);

                if (allowed)
                {
                    _d.CanSpawn = false;
                    _d.PoolManager.ReuseObject(_d.IceSlabPrefab, position, rotation);
                }
                else
                {
                    _d.PoolManager.ReuseObject(_d.RedShapePrefab, position, rotation);
                }
            }

            public void PerformDash()
            {
                _d.Rigidbody.gravityScale = _d.DefaultGravityScale;
                _d.Rigidbody.velocity = new(_d.Rigidbody.velocity.x, 0);
                _d.Rigidbody.AddForce(new Vector2(_d.FacingRight ? 1 : -1, 0) * _d.DashForce, ForceMode2D.Impulse);

                _d.CanDash = false;
                _d.InputTimer = _d.DashDuration;
                _d.CurrentLocomotion = LocomotionState.Dashing;
                _d.AudioSystem.PlaySound(_d.DashData, _d.AudioSource);
                _d.PoolManager.ReuseObject(_d.DashEffect, _d.Transform.position, Quaternion.identity);
            }

            public void PerformBounce()
            {
                _d.InputTimer = _d.BounceTime;
                Vector2 bounceDirection = Vector2.up;

                if (_d.LastTouchedWall == DirectionType.Left)
                {
                    bounceDirection += Vector2.right;
                }
                else if (_d.LastTouchedWall == DirectionType.Right)
                {
                    bounceDirection += Vector2.left;
                }

                bounceDirection.Normalize();
                bounceDirection = new(bounceDirection.x, bounceDirection.y * _d.VerticalBounceMultiplier);

                PerformJump(LocomotionState.Bouncing, bounceDirection, Vector2.zero);
            }

            public void PerformJump(LocomotionState state, Vector2 direction, Vector2 velocity)
            {
                _d.Rigidbody.velocity = velocity;
                _d.Rigidbody.gravityScale = _d.DefaultGravityScale;
                _d.Rigidbody.AddForce(direction * _d.JumpForce, ForceMode2D.Impulse);

                _d.PoolManager.ReuseObject(_d.JumpEffect, _d.Transform.position, Quaternion.identity);
                _d.AudioSystem.PlaySound(_d.JumpData, _d.AudioSource);
                _d.CurrentLocomotion = state;
                _d.LastGroundedTime = 0f;
                _d.LastHangingTime = 0;
                _d.LastJumpTime = 0f;
            }

            public void PerformAirJump(Vector2 direction)
            {
                _d.CurrentAirJumps++;

                PerformJump(LocomotionState.Jumping, direction, new Vector2 (_d.Rigidbody.velocity.x, 0));
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

            public float CalculateMovement(float input)
            {
                float targetSpeed = input * _d.MovementSpeed;
                float speeddifference = targetSpeed - _d.Rigidbody.velocity.x;
                float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01) ? _d.Acceleration : _d.Deceleration;
                float turnRateMultiplier = _d.CurrentLocomotion == LocomotionState.Grounded ? 1 : _d.AirMultiplier;
                return Mathf.Pow(Mathf.Abs(speeddifference) * accelerationRate * turnRateMultiplier, _d.VelocityPower) * Mathf.Sign(speeddifference);
            }

            public bool CanSpawn()
            {
                return _d.CanSpawn;
            }

            public bool CanDash(LocomotionState state)
            {
                return _d.CanSpawn && state != LocomotionState.Dashing && state != LocomotionState.Hanging;
            }

            public bool CanAirJump(LocomotionState state)
            {
                return (_d.CurrentAirJumps < _d.MaximumAirJumps) && state != LocomotionState.Grounded && state != LocomotionState.Hanging;
            }

            public bool DirectionOpposite(float direction)
            {
                return (_d.FacingRight && direction < 0) || (!_d.FacingRight && direction > 0);
            }

            private (Vector2 position, bool allowed) FindNearestValidPosition(Vector2 start, Quaternion rotation)
            {
                Vector2[] directions = { Vector2.up, Vector2.down };
                Vector2 currentPosition = start;
                Vector2 boxSize = new(3, 1);
                const int maxAttempts = 4;
                const float stepDistance = 0.5f;
                int groundLayer = _d.GroundLayer;

                for (int i = 0; i < maxAttempts; i++)
                {
                    foreach (var direction in directions)
                    {
                        Vector2 testPosition = currentPosition + i * stepDistance * direction;
                        if (!Physics2D.OverlapBox(testPosition, boxSize, rotation.eulerAngles.z, groundLayer))
                        {
                            return (testPosition, true);
                        }
                    }
                }
                return (start, false);
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

            public (LocomotionState previousLocomotion, LocomotionState currentLocomotion)
            UpdateLocomotionState(LocomotionState currentLocomotion, float verticalVelocity, bool grounded, bool hanging)
            {
                LocomotionState newLocomotion = currentLocomotion;

                if (grounded)
                {
                    newLocomotion = LocomotionState.Grounded;
                }
                else if (hanging)
                {
                    newLocomotion = LocomotionState.Hanging;
                }
                else if (verticalVelocity < 0)
                {
                    newLocomotion = LocomotionState.Falling;
                }

                return (currentLocomotion, newLocomotion);
            }

            public (bool grounded, bool hanging)
            HandlePhysicsChecks()
            {
                (Vector2 groundCheckOrigin, Vector2 leftWallCheckOrigin, Vector2 rightWallCheckOrigin) =
                GetPhysicsOrigins(_d.BoxCollider, _d.Transform, _d.WallCheckOffset);

                Vector2 groundCheckSize = new(_d.BoxCollider.bounds.size.x - _d.GroundCheckDistance, _d.GroundCheckDistance);
                Collider2D rightWallHit = Physics2D.OverlapCircle(rightWallCheckOrigin, _d.GrabCheckRadius, _d.GroundLayer);
                Collider2D leftWallHit = Physics2D.OverlapCircle(leftWallCheckOrigin, _d.GrabCheckRadius, _d.GroundLayer);
                Collider2D groundHit = Physics2D.OverlapBox(groundCheckOrigin, groundCheckSize, 0, _d.GroundLayer);
                bool grounded = groundHit;
                bool hanging = false;

                if (leftWallHit && !_d.FacingRight)
                {
                    hanging = true;
                    _d.LastTouchedWall = DirectionType.Left;
                }
                else if (rightWallHit && _d.FacingRight)
                {
                    hanging = true;
                    _d.LastTouchedWall = DirectionType.Right;
                }

                if (grounded)
                {
                    _d.LastGroundedTime = _d.JumpCoyoteTime;

                    if (!groundHit.CompareTag("Ice"))
                        _d.RefreshAbilities();
                }
                else if (hanging)
                {
                    _d.LastHangingTime = _d.JumpCoyoteTime;
                }

                return (grounded, hanging);
            }
        }
    }
}