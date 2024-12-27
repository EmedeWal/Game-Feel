using UnityEngine;

namespace ShatterStep
{
    namespace Player
    {
        [RequireComponent(typeof(BoxCollider2D))]
        [RequireComponent(typeof(Rigidbody2D))]
        public class Controller
        {
            public Handler Handler { get; private set; }
            public Data Data { get; private set; }

            public Controller(Data data)
            {
                Data = data;

                SubscribeToActions(true);

                Handler = new(Data);
            }

            public void Cleanup()
            {
                SubscribeToActions(false);
            }

            public void FixedTick(float deltaTime, float horizontalInput)
            {
                Data.PreviousVelocity = Data.Rigidbody.velocity;

                HandleAbilities();

                float verticalVelocity = Data.Rigidbody.velocity.y;
                float inputMagnitude = Mathf.Abs(horizontalInput);

                if (Data.InputTimer <= 0)
                {
                    UpdateLocomotionState(verticalVelocity);
                }

                HandlePhysics(inputMagnitude);
                HandleVisuals(inputMagnitude, horizontalInput);

                if (Data.InputTimer <= 0)
                {
                    HandleVerticalMovement();
                    HandleHorizontalMovement(horizontalInput);
                }

                Data.FixedTick(deltaTime);
            }

            private void HandleAbilities()
            {
                if (Data.SpawnPressed && Handler.CanSpawn())
                {
                    Handler.PerformSpawn(Data.BoxCollider);
                }
                else if (Data.DashPressed && Handler.CanDash(Data.CurrentLocomotion))
                {
                    Handler.PerformDash();
                }

                if (Data.CurrentLocomotion == LocomotionState.Dashing && Data.InputTimer <= 0)
                {
                    Data.Rigidbody.gravityScale = Data.DefaultGravityScale;
                    Data.CurrentLocomotion = LocomotionState.Falling;
                }
            }

            private void UpdateLocomotionState(float verticalVelocity)
            {
                (bool grounded, bool hanging) = Handler.HandlePhysicsChecks();
                (Data.PreviousLocomotion, Data.CurrentLocomotion) = Handler.UpdateLocomotionState(Data.CurrentLocomotion, verticalVelocity, grounded, hanging);
            }

            private void HandlePhysics(float inputMagnitude)
            {
                #region Transitions
                if (Data.PreviousLocomotion != LocomotionState.Hanging && Data.CurrentLocomotion == LocomotionState.Hanging)
                {
                    Data.Rigidbody.gravityScale = Data.HangingGravityScale;
                    Data.Rigidbody.velocity = new(Data.Rigidbody.velocity.x, 0);
                }
                else if (Data.PreviousLocomotion == LocomotionState.Hanging && Data.CurrentLocomotion != LocomotionState.Hanging)
                {
                    Data.Rigidbody.gravityScale = Data.DefaultGravityScale;
                }
                #endregion

                #region Gravity
                if (Data.CurrentLocomotion == LocomotionState.Dashing)
                {
                    Data.Rigidbody.gravityScale = 0;
                }
                else if (Data.CurrentLocomotion == LocomotionState.Grounded)
                {
                    Data.Rigidbody.gravityScale = Data.DefaultGravityScale;
                }
                else if (Data.CurrentLocomotion == LocomotionState.Falling)
                {
                    Data.Rigidbody.gravityScale += Data.FallGravityIncrement;
                }
                else if (Data.CurrentLocomotion == LocomotionState.Hanging)
                {
                    Data.Rigidbody.gravityScale += Data.FallGravityIncrement * Data.HangingGravityMultiplier;
                }
                #endregion

                #region Friction
                if (inputMagnitude < 0.01f)
                {
                    float amount = Mathf.Min(Mathf.Abs(Data.Rigidbody.velocity.x), Mathf.Abs(Data.Friction));
                    amount *= Mathf.Sign(Data.Rigidbody.velocity.x);

                    Data.Rigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
                }

                if (Mathf.Abs(Data.Rigidbody.velocity.x) < 0.05)
                {
                    Data.Rigidbody.velocity = new Vector2(0, Data.Rigidbody.velocity.y);
                }
                #endregion

                float newVerticalVelocity = Mathf.Clamp(Data.Rigidbody.velocity.y, -Data.MaximumVerticalVelocity, Data.MaximumVerticalVelocity);
                Data.Rigidbody.velocity = new(Data.Rigidbody.velocity.x, newVerticalVelocity);
            }

            private void HandleVisuals(float inputMagnitude, float horizontalInput)
            {
                Data.DustParticles.rateOverTime = Data.CurrentLocomotion == LocomotionState.Grounded ? Data.Particles : 0;

                float direction = Data.InputTimer <= 0 ? horizontalInput : Data.Rigidbody.velocity.x;

                if (Handler.DirectionOpposite(direction))
                {
                    Handler.TurnCharacter();
                }

                Handler.FixedTick(inputMagnitude);
            }

            private void HandleVerticalMovement()
            {
                if (Data.LastJumpTime > 0)
                {
                    if (Data.LastHangingTime > 0)
                    {
                        Handler.PerformBounce();
                    }
                    else if (Data.LastGroundedTime > 0)
                    {
                        Handler.PerformJump(LocomotionState.Jumping, Vector2.up, new Vector2(Data.Rigidbody.velocity.x, 0));
                    }
                    else if (Handler.CanAirJump(Data.CurrentLocomotion))
                    {
                        Handler.PerformAirJump(Vector2.up * Data.AirJumpMultiplier);
                    }
                }
            }

            private void HandleHorizontalMovement(float horizontalInput)
            {
                float movement = Handler.CalculateMovement(horizontalInput);
                Data.Rigidbody.AddForce(movement * Vector2.right);
            }

            #region Input Handling
            public void SubscribeToActions(bool subscribe)
            {
                if (subscribe)
                {
                    if (Data.InputManager.GetAction(ActionType.Jump, out var JumpAction))
                    {
                        JumpAction.Performed += Locomotion_JumpInputPerformed;
                        JumpAction.Canceled += Locomotion_JumpInputCanceled;
                    }

                    if (Data.InputManager.GetAction(ActionType.Dash, out var DashAction))
                    {
                        DashAction.Performed += Locomotion_DashInputPerformed;
                    }

                    if (Data.InputManager.GetAction(ActionType.Spawn, out var SpawnAction))
                    {
                        SpawnAction.Performed += Locomotion_SpawnInputPerformed;
                    }
                }
                else
                {
                    if (Data.InputManager.GetAction(ActionType.Jump, out var JumpAction))
                    {
                        JumpAction.Performed -= Locomotion_JumpInputPerformed;
                        JumpAction.Canceled -= Locomotion_JumpInputCanceled;
                    }

                    if (Data.InputManager.GetAction(ActionType.Dash, out var DashAction))
                    {
                        DashAction.Performed -= Locomotion_DashInputPerformed;
                    }

                    if (Data.InputManager.GetAction(ActionType.Spawn, out var SpawnAction))
                    {
                        SpawnAction.Performed -= Locomotion_SpawnInputPerformed;
                    }
                }
            }

            private void Locomotion_JumpInputPerformed()
            {
                Data.LastJumpTime = Data.JumpBufferTime;
            }

            private void Locomotion_JumpInputCanceled()
            {
                if (Data.CurrentLocomotion == LocomotionState.Jumping)
                {
                    Handler.CutVerticalVelocity();
                }
            }

            private void Locomotion_DashInputPerformed()
            {
                Data.DashPressed = true;
            }

            private void Locomotion_SpawnInputPerformed()
            {
                Data.SpawnPressed = true;
            }
            #endregion
        }
    }
}