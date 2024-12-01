using UnityEngine;
using System;

namespace ShatterStep
{
    namespace Player
    {
        [CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable Object/Data/Player")]
        public class Data : ScriptableObject
        {
            public AudioSystem AudioSystem { get; private set; }
            public PoolManager PoolManager { get; private set; }

            [Header("AUDIO SETTINGS")]
            public AudioData JumpData;
            public AudioData DashData;
            public AudioData DeathData;

            [Header("VISUAL SETTINGS")]
            public PoolObject JumpEffect;
            public PoolObject DashEffect;
            public Color RespawnColor; 
            public float RespawnTime = 0.5f;
            public float Particles = 35f;

            [Header("SPEED SETTINGS")]
            public float Friction = 0.25f;
            public float MovementSpeed = 9f;
            public float VelocityPower = 0.95f;
            public float Acceleration = 16f;
            public float Deceleration = 13f;

            [Header("JUMP SETTINGS")]
            [Range(1, 2)] public float VerticalBounceMultiplier = 1.2f;
            [Range(0, 1)] public float JumpCutMultiplier = 0.6f;
            [Range(0, 1)] public float AirJumpMultiplier = 0.8f;
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
            [Range(0, 1)] public float HangingGravityMultiplier = 0.1f;
            [Range(0, 1)] public float HangingGravityScale = 0.1f;
            public float MaximumVerticalVelocity = 60f;
            public float FallGravityIncrement = 0.02f;
            public float DefaultGravityScale = 2f;

            [Header("OVERLAP CIRCLE SETTINGS")]
            public Vector2 WallCheckOffset = new(0.25f, 0f);
            public float GroundCheckRadius = 0.2f;
            public float GrabCheckRadius = 0.2f;

            [Header("SPAWN SETTINGS")]
            public PoolObject IceSlabPrefab;
            public PoolObject RedShapePrefab;

            [HideInInspector] public LocomotionState PreviousLocomotion;
            [HideInInspector] public LocomotionState CurrentLocomotion;
            [HideInInspector] public DirectionType LastJumpDirection;
            [HideInInspector] public DirectionType LastTouchedWall;
            [HideInInspector] public int CurrentAirJumps;
            [HideInInspector] public float LastGroundedTime;
            [HideInInspector] public float LastHangingTime;
            [HideInInspector] public float LastJumpTime;
            [HideInInspector] public float InputTimer;
            [HideInInspector] public bool FacingRight;
            [HideInInspector] public bool SpawnPressed;
            [HideInInspector] public bool DashPressed;
            [HideInInspector] public bool CanSpawn;
            [HideInInspector] public bool CanDash;

            [HideInInspector] public ParticleSystem.EmissionModule DustParticles;
            [HideInInspector] public ApplicationManager ApplicationManager;
            [HideInInspector] public InputManager InputManager;
            [HideInInspector] public AnimatorManager AnimatorManager;
            [HideInInspector] public BoxCollider2D BoxCollider;
            [HideInInspector] public AudioSource AudioSource;
            [HideInInspector] public Rigidbody2D Rigidbody;
            [HideInInspector] public Controller Controller;
            [HideInInspector] public Transform Transform;
            [HideInInspector] public LayerMask GroundLayer;
            [HideInInspector] public Vector2 RespawnPoint;

            public event Action PlayerDeath;

            public void Init(GameObject playerObject, InputManager inputHandler)
            {
                ApplicationManager = ApplicationManager.Instance;
                AudioSystem = AudioSystem.Instance;
                PoolManager = PoolManager.Instance;

                PoolManager.CreatePool(DashEffect, 5);
                PoolManager.CreatePool(JumpEffect, 5);
                PoolManager.CreatePool(IceSlabPrefab, 5);
                PoolManager.CreatePool(RedShapePrefab, 1);

                InputManager = inputHandler;
                DustParticles = playerObject.GetComponentInChildren<ParticleSystem>().emission;
                BoxCollider = playerObject.GetComponent<BoxCollider2D>();
                AudioSource = playerObject.GetComponent<AudioSource>();
                Rigidbody = playerObject.GetComponent<Rigidbody2D>();
                Transform = playerObject.transform;
                GroundLayer = LayerMask.GetMask("Ground");
                RespawnPoint = Transform.position;

                AudioSource.playOnAwake = false;

                Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                Rigidbody.gravityScale = DefaultGravityScale;
                Rigidbody.isKinematic = false;

                FacingRight = true;

                ResetState();
            }

            public void Setup(Controller controller, AnimatorManager animatorManager)
            {
                Controller = controller;
                AnimatorManager = animatorManager;
            }

            public void FixedTick(float fixedDeltaTime)
            {
                LastGroundedTime -= fixedDeltaTime;
                LastHangingTime -= fixedDeltaTime;
                LastJumpTime -= fixedDeltaTime;
                InputTimer -= fixedDeltaTime;

                SpawnPressed = false;
                DashPressed = false;
            }

            public void RespawnPlayer(Vector2 position)
            {
                PreviousLocomotion = LocomotionState.Grounded;
                CurrentLocomotion = LocomotionState.Grounded;
                Rigidbody.velocity = Vector2.zero;
                Rigidbody.gravityScale = 0;
                RespawnPoint = position;

                ApplicationManager.SetGameState(GameState.Paused, RespawnTime);
                ApplicationManager.GameStateUpdated += Data_GameStateUpdated;

                AudioSystem.Play(DeathData, AudioSource);
                Controller.SubscribeToActions(false);

                OnPlayerDeath();
                ResetState();
            }

            public void RefreshAbilities()
            {
                CurrentAirJumps = 0;
                CanSpawn = true;
                CanDash = true;
            }

            private void Data_GameStateUpdated(GameState gameState)
            {
                ApplicationManager.GameStateUpdated -= Data_GameStateUpdated;
                Transform.position = RespawnPoint;

                Controller.SubscribeToActions(true);
            }

            private void OnPlayerDeath()
            {
                PlayerDeath?.Invoke();
            }

            private void ResetState()
            {
                PreviousLocomotion = LocomotionState.Grounded;
                CurrentLocomotion = LocomotionState.Grounded;
                LastTouchedWall = DirectionType.None;

                InputTimer = 0;
                CurrentAirJumps = 0;
                SpawnPressed = false;
                DashPressed = false;

                LastGroundedTime = 0;
                LastHangingTime = 0;
                LastJumpTime = 0;

                CanSpawn = true;
                CanDash = true;
            }
        }
    }
}