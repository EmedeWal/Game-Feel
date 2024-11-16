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
                public float MovementSpeed = 9f;
                public float VelocityPower = 0.95f;
                public float Acceleration = 16f;
                public float Deceleration = 13f;
                public float DragAmount = 2f;

                [Header("JUMP SETTINGS")]
                [Range(0, 1)] public float JumpCutMultiplier = 0.6f;
                public float JumpCoyoteTime = 0.15f;
                public float JumpBufferTime = 0.1f;
                public float JumpForce = 13f;

                [Header("GRAVITY SETTINGS")]
                public float DefaultGravityScale = 1f;
                public float FallGravityMultiplier = 1.05f;

                [Header("OVERLAP CIRCLE SETTINGS")]
                public Vector2 WallCheckOffset = new(0.25f, 0.5f);
                public float GroundCheckRadius = 0.05f;
                public float GrabCheckRadius = 0.2f;

                [HideInInspector] public float LastGroundedTime = 0f;
                [HideInInspector] public float LastHangingTime = 0f;
                [HideInInspector] public float LastJumpTime = 0f;
                [HideInInspector] public bool FacingRight = true;

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
                }

                public void FixedTick(float deltaTime)
                {
                    LastGroundedTime -= deltaTime;
                    LastHangingTime -= deltaTime;
                    LastJumpTime -= deltaTime;
                }
            }
        }
    }
}