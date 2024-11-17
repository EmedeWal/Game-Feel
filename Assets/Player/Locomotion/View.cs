using UnityEngine;

namespace Custom
{
    namespace Player
    {
        namespace Locomotion
        {
            [CreateAssetMenu(fileName = "Locomotion View", menuName = "Scriptable Object/View/Locomotion")]
            public class View : ScriptableObject
            {
                [Header("SPEED SETTINGS")]
                [Range(0, 1)] public float AirMultiplier = 0.6f;
                public float FrictionAmount = 0.25f;
                public float MovementSpeed = 9f;
                public float VelocityPower = 0.95f;
                public float Acceleration = 16f;
                public float Deceleration = 13f;

                [Header("JUMP SETTINGS")]
                [Range(1, 2)] public float WallJumpMultiplier = 1.2f;
                [Range(0, 1)] public float JumpCutMultiplier = 0.6f;
                public int MaximumAirJumps = 1;
                public float JumpCoyoteTime = 0.15f;
                public float JumpBufferTime = 0.1f;
                public float WallJumpTime = 0.2f;
                public float JumpForce = 13f;

                [Header("DASH SETTINGS")]
                public float DashForce = 15f;
                public float DashCooldown = 3f;
                public float DashDuration = 0.1f;

                [Header("GRAVITY SETTINGS")]
                [Range(0, 1)] public float GravityScaleCutMultiplier = 0.5f;
                public float MaximumVerticalVelocity = 30f;
                public float FallGravityMultiplier = 1.05f;
                public float DefaultGravityScale = 1f;

                [Header("OVERLAP CIRCLE SETTINGS")]
                public Vector2 WallCheckOffset = new(0.25f, 0.5f);
                public float GroundCheckRadius = 0.05f;
                public float GrabCheckRadius = 0.2f;

                [HideInInspector] public Direction LastTouchedWall;
                [HideInInspector] public int CurrentAirJumps;
                [HideInInspector] public float LastGroundedTime;
                [HideInInspector] public float LastHangingTime;
                [HideInInspector] public float LastJumpTime;
                [HideInInspector] public float InputTimer;
                [HideInInspector] public float DashTimer;
                [HideInInspector] public bool FacingRight;
                [HideInInspector] public bool HandleInput;

                [HideInInspector] public InputHandler InputHandler;
                [HideInInspector] public AnimatorManager AnimatorManager;
                [HideInInspector] public BoxCollider2D BoxCollider;
                [HideInInspector] public Rigidbody2D Rigidbody;
                [HideInInspector] public Transform Transform;
                [HideInInspector] public LayerMask IgnoreLayers;

                public void Init(GameObject playerObject)
                {
                    int controllerLayer = LayerMask.NameToLayer("Controller");
                    int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");
                    IgnoreLayers = ~(1 << controllerLayer | 1 << damageColliderLayer);

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