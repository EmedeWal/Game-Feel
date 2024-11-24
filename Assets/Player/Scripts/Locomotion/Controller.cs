using UnityEngine;

namespace Custom
{
    namespace Player
    {
        [RequireComponent(typeof(BoxCollider2D))]
        [RequireComponent(typeof(Rigidbody2D))]
        public class Controller
        {
            public Handler Handler { get; private set; }
            public Data Data { get; private set; }

            public Controller(Data data, GameObject gameObject, InputHandler inputHandler, AnimatorManager animatorManager)
            {
                Data = data;

                Data.Init(gameObject, inputHandler, animatorManager);

                Data.InputHandler.JumpInputPerformed += Locomotion_JumpInputPerformed;
                Data.InputHandler.JumpInputCanceled += Locomotion_JumpInputCanceled;
                Data.InputHandler.DashInputPerformed += Locomotion_DashInputPerformed;

                Handler = new(Data);
            }

            public void Cleanup()
            {
                Handler = null;

                Data.InputHandler.JumpInputPerformed -= Locomotion_JumpInputPerformed;
                Data.InputHandler.JumpInputCanceled -= Locomotion_JumpInputCanceled;
                Data.InputHandler.DashInputPerformed -= Locomotion_DashInputPerformed;

                Data = null;
            }

            public void FixedTick(float deltaTime, float horizontalInput)
            {
                float inputMagnitude = Mathf.Abs(horizontalInput);
                UpdateLocomotionState(Data.Rigidbody.velocity.y);

                HandlePhysics(inputMagnitude);
                HandleVisuals(inputMagnitude);

                if (Data.HandleInput)
                {
                    HandleVerticalMovement();
                    HandleHorizontalMovement(horizontalInput);
                }

                Data.FixedTick(deltaTime);
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
                if (Handler.CanDash(Data.CurrentLocomotion))
                {
                    Handler.PerformDash();
                }
            }

            private void UpdateLocomotionState(float verticalVelocity)
            {
                if (!Data.HandleInput) return;
                (bool grounded, bool hanging) = Handler.HandlePhysicsChecks();
                (Data.PreviousLocomotion, Data.CurrentLocomotion) = Handler.UpdateLocomotionState(Data.CurrentLocomotion, verticalVelocity, grounded, hanging);
            }

            private void HandleHorizontalMovement(float horizontalInput)
            {
                float movement = Handler.CalculateMovement(horizontalInput);
                Data.Rigidbody.AddForce(movement * Vector2.right);
            }

            private void HandleVerticalMovement()
            {
                if (Data.CurrentLocomotion == LocomotionState.Grounded)
                {
                    Data.CurrentAirJumps = 0;
                }

                if (Data.LastJumpTime > 0)
                {
                    if (Data.LastHangingTime > 0)
                    {
                        Handler.PerformBounce();
                    }
                    else if (Data.LastGroundedTime > 0)
                    {
                        Handler.PerformJump(LocomotionState.Jumping, Vector2.up);
                    }
                    else if (Handler.CanAirJump(Data.CurrentLocomotion))
                    {
                        Handler.PerformAirJump(Vector2.up * Data.JumpCutMultiplier);
                    }
                }
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
                else if (Data.CurrentLocomotion == LocomotionState.Falling || Data.CurrentLocomotion == LocomotionState.Hanging)
                {
                    Data.Rigidbody.gravityScale += Data.FallGravityIncrement;
                }
                #endregion

                #region Friction
                if (Data.CurrentLocomotion == LocomotionState.Grounded && Mathf.Abs(inputMagnitude) < 0.01f)
                {
                    float amount = Mathf.Min(Mathf.Abs(Data.Rigidbody.velocity.x), Mathf.Abs(Data.FrictionAmount));
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

            private void HandleVisuals(float inputMagnitude)
            {
                Data.DustParticles.rateOverTime = Data.CurrentLocomotion == LocomotionState.Grounded ? Data.Particles : 0;

                if (Handler.DirectionOpposite(Data.Rigidbody.velocity.x))
                {
                    Handler.TurnCharacter();
                }

                Handler.FixedTick(inputMagnitude);
            }
        }
    }
}