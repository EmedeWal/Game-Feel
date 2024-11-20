using UnityEngine;

namespace Custom
{
    namespace Player
    {
        namespace Locomotion
        {
            [CreateAssetMenu(fileName = "Locomotion Data", menuName = "Scriptable Object/Data/Locomotion")]
            public class Data : ScriptableObject
            {
                public PoolManager PoolManager { get; private set; }

                [Header("VISUAL SETTINGS")]
                public PoolObject JumpEffect;
                public PoolObject DashEffect;
                public float Particles = 70f;

                [Header("SPEED SETTINGS")]
                public float FrictionAmount = 0.25f;
                public float MovementSpeed = 9f;
                public float VelocityPower = 0.95f;
                public float Acceleration = 16f;
                public float Deceleration = 13f;

                [Header("JUMP SETTINGS")]
                [Range(1, 2)] public float WallJumpMultiplier = 1.4f;
                [Range(0, 1)] public float JumpCutMultiplier = 0.6f;
                [Range(0, 1)] public float AirMultiplier = 0.4f;
                public int MaximumAirJumps = 1;
                public float JumpCoyoteTime = 0.15f;
                public float JumpBufferTime = 0.1f;
                public float WallJumpTime = 0.3f;
                public float JumpForce = 21f;

                [Header("DASH SETTINGS")]
                public float DashForce = 33f;
                public float DashCooldown = 3f;
                public float DashDuration = 0.1f;

                [Header("GRAVITY SETTINGS")]
                public float MaximumVerticalVelocity = 30f;
                public float FallGravityIncrement = 0.01f;
                public float DefaultGravityScale = 1f;

                [Header("OVERLAP CIRCLE SETTINGS")]
                public Vector2 WallCheckOffset = new(0.25f, 0.5f);
                public float GroundCheckRadius = 0.2f;
                public float GrabCheckRadius = 0.2f;

                [HideInInspector] public Direction LastJumpDirection;
                [HideInInspector] public Direction LastTouchedWall;
                [HideInInspector] public int CurrentAirJumps;
                [HideInInspector] public float LastGroundedTime;
                [HideInInspector] public float LastHangingTime;
                [HideInInspector] public float LastJumpTime;
                [HideInInspector] public float InputTimer;
                [HideInInspector] public float DashTimer;
                [HideInInspector] public bool FacingRight;
                [HideInInspector] public bool HandleInput;

                [HideInInspector] public ParticleSystem.EmissionModule DustParticles;
                [HideInInspector] public InputHandler InputHandler;
                [HideInInspector] public AnimatorManager AnimatorManager;
                [HideInInspector] public BoxCollider2D BoxCollider;
                [HideInInspector] public Rigidbody2D Rigidbody;
                [HideInInspector] public Transform Transform;
                [HideInInspector] public LayerMask WallLayer;
                [HideInInspector] public LayerMask GroundedLayers;

                public void Init(GameObject playerObject)
                {
                    PoolManager = PoolManager.Instance;

                    PoolManager.CreatePool(DashEffect, 2);
                    PoolManager.CreatePool(JumpEffect, 2);

                    int wallLayer = LayerMask.NameToLayer("Wall");
                    int groundLayer = LayerMask.NameToLayer("Ground");
                    GroundedLayers = 1 << groundLayer | 1 << wallLayer;
                    WallLayer = 1 << wallLayer;

                    DustParticles = playerObject.GetComponentInChildren<ParticleSystem>().emission;
                    InputHandler = playerObject.GetComponent<InputHandler>();
                    AnimatorManager = playerObject.GetComponent<AnimatorManager>();
                    BoxCollider = playerObject.GetComponent<BoxCollider2D>();
                    Rigidbody = playerObject.GetComponent<Rigidbody2D>();
                    Transform = playerObject.transform;

                    Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    Rigidbody.gravityScale = DefaultGravityScale;
                    Rigidbody.isKinematic = false;

                    LastTouchedWall = Direction.None;
                    CurrentAirJumps = 0;
                    LastGroundedTime = 0;
                    LastHangingTime = 0;
                    LastJumpTime = 0;
                    InputTimer = 0;
                    DashTimer = 0;
                    FacingRight = true;
                    HandleInput = true;
                }

                public void FixedTick(float deltaTime)
                {
                    LastGroundedTime -= deltaTime;
                    LastHangingTime -= deltaTime;
                    LastJumpTime -= deltaTime;
                    InputTimer -= deltaTime;
                    DashTimer -= deltaTime;
                    
                    CheckReset();
                }

                public void DisableInput(float duration)
                {
                    HandleInput = false;
                    InputTimer = duration;
                    Rigidbody.velocity = Vector2.zero;
                }

                private void CheckReset()
                {
                    if (InputTimer < 0 && !HandleInput)
                    {
                        HandleInput = true;
                        Rigidbody.gravityScale = DefaultGravityScale;
                    }
                }
            }
        }
    }
}