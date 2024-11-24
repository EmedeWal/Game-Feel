using UnityEngine;
using System;

namespace Custom
{
    namespace Player
    {
        [CreateAssetMenu(fileName = "Locomotion Data", menuName = "Scriptable Object/Data/Locomotion")]
        public class Data : ScriptableObject
        {
            public AudioSystem AudioSystem { get; private set; }
            public PoolManager PoolManager { get; private set; }

            [Header("AUDIO SETTINGS")]
            public AudioData JumpData;
            public AudioData DashData;

            [Header("VISUAL SETTINGS")]
            public PoolObject JumpEffect;
            public PoolObject DashEffect;
            public float Particles = 35f;

            [Header("SPEED SETTINGS")]
            public float FrictionAmount = 0.25f;
            public float MovementSpeed = 9f;
            public float VelocityPower = 0.95f;
            public float Acceleration = 16f;
            public float Deceleration = 13f;

            [Header("JUMP SETTINGS")]
            [Range(0, 1)] public float JumpCutMultiplier = 0.6f;
            [Range(0, 1)] public float BounceMultiplier = 0.1f;
            [Range(0, 1)] public float AirMultiplier = 0.4f;
            public int MaximumAirJumps = 1;
            public float JumpCoyoteTime = 0.15f;
            public float JumpBufferTime = 0.1f;
            public float BounceTime = 0.1f;
            public float JumpForce = 21f;

            [Header("DASH SETTINGS")]
            public float DashForce = 26f;
            public float DashCooldown = 2f;
            public float DashDuration = 0.1f;

            [Header("GRAVITY SETTINGS")]
            [Range(0, 1)] public float HangingGravityScale = 0.4f;
            public float MaximumVerticalVelocity = 60f;
            public float FallGravityIncrement = 0.01f;
            public float DefaultGravityScale = 2f;

            [Header("OVERLAP CIRCLE SETTINGS")]
            public Vector2 WallCheckOffset = new(0.25f, 0f);
            public float GroundCheckRadius = 0.2f;
            public float GrabCheckRadius = 0.2f;

            [HideInInspector] public Vector2 LastGroundedPosition;
            [HideInInspector] public LocomotionState PreviousLocomotion;
            [HideInInspector] public LocomotionState CurrentLocomotion;
            [HideInInspector] public DirectionType LastJumpDirection;
            [HideInInspector] public DirectionType LastTouchedWall;
            [HideInInspector] public int CurrentAirJumps;
            [HideInInspector] public float LastGroundedTime;
            [HideInInspector] public float LastHangingTime;
            [HideInInspector] public float LastJumpTime;
            [HideInInspector] public float InputTimer;
            [HideInInspector] public float DashTimer;
            [HideInInspector] public bool DashOffCooldown;
            [HideInInspector] public bool FacingRight;
            [HideInInspector] public bool HandleInput;

            [HideInInspector] public ParticleSystem.EmissionModule DustParticles;
            [HideInInspector] public InputHandler InputHandler;
            [HideInInspector] public AnimatorManager AnimatorManager;
            [HideInInspector] public BoxCollider2D BoxCollider;
            [HideInInspector] public Rigidbody2D Rigidbody;
            [HideInInspector] public Transform Transform;
            [HideInInspector] public LayerMask GroundLayer;

            public event Action<float> DashCooldownStarted;
            public event Action DashCooldownFinished;

            public void Init(GameObject playerObject, InputHandler inputHandler, AnimatorManager animatorManager)
            {
                AudioSystem = AudioSystem.Instance;
                PoolManager = PoolManager.Instance;

                PoolManager.CreatePool(DashEffect, 2);
                PoolManager.CreatePool(JumpEffect, 2);

                InputHandler = inputHandler;
                AnimatorManager = animatorManager;
                DustParticles = playerObject.GetComponentInChildren<ParticleSystem>().emission;
                BoxCollider = playerObject.GetComponent<BoxCollider2D>();
                Rigidbody = playerObject.GetComponent<Rigidbody2D>();
                Transform = playerObject.transform;
                GroundLayer = LayerMask.GetMask("Ground");

                Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                Rigidbody.gravityScale = DefaultGravityScale;
                Rigidbody.isKinematic = false;

                PreviousLocomotion = LocomotionState.Grounded;
                CurrentLocomotion = LocomotionState.Grounded;
                LastTouchedWall = DirectionType.None;
                CurrentAirJumps = 0;
                LastGroundedTime = 0;
                LastHangingTime = 0;
                LastJumpTime = 0;
                InputTimer = 0;
                DashTimer = 0;
                DashOffCooldown = true;
                FacingRight = true;
                HandleInput = true;
            }

            public void FixedTick(float fixedDeltaTime)
            {
                LastGroundedTime -= fixedDeltaTime;
                LastHangingTime -= fixedDeltaTime;
                LastJumpTime -= fixedDeltaTime;

                TickInput(fixedDeltaTime);
                TickDash(fixedDeltaTime);
            }

            public void OnDashCooldownStarted(float cooldownTime)
            {
                DashCooldownStarted?.Invoke(cooldownTime);
                DashTimer = cooldownTime;
            }

            public void OnDashCooldownFinished()
            {
                DashCooldownFinished?.Invoke();
            }

            public void DisableInput(float duration)
            {
                HandleInput = false;
                InputTimer = duration;
            }

            private void TickInput(float fixedDeltaTime)
            {
                if (InputTimer > 0)
                {
                    InputTimer -= fixedDeltaTime;
                }
                else if (!HandleInput)
                {
                    HandleInput = true;
                    Rigidbody.gravityScale = DefaultGravityScale;
                }
            }

            private void TickDash(float fixedDeltaTime)
            {
                if (InputTimer > 0)
                {
                    DashTimer -= fixedDeltaTime;
                }
                else if (!DashOffCooldown)
                {
                    DashOffCooldown = true;
                    OnDashCooldownFinished();
                }
            }
        }
    }
}