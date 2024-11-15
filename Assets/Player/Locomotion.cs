using Custom.Player.Utilities;
using UnityEngine;

namespace Custom
{
    namespace Player
    {
        [RequireComponent(typeof(BoxCollider2D))]
        [RequireComponent(typeof(Rigidbody2D))]
        public class Locomotion : MonoBehaviour
        {
            private LocomotionGetters _getters;
            [SerializeField] private LocomotionState _state;

            [Header("SPEED SETTINGS")]
            [SerializeField] private float _frictionAmount = 0.25f;
            [SerializeField] private float _movementSpeed = 9f;
            [SerializeField] private float _velocityPower = 0.95f;
            [SerializeField] private float _acceleration = 16f;
            [SerializeField] private float _deceleration = 13f;

            [Header("JUMP SETTINGS")]
            [Range(0, 1)][SerializeField] private float _jumpCutMultiplier = 0.6f;
            [SerializeField] private float _jumpCoyoteTime = 0.15f;
            [SerializeField] private float _jumpBufferTime = 0.1f;
            [SerializeField] private float _jumpForce = 13f;
            private float _lastGroundedTime = 0f;
            private float _lastJumpTime = 0f;

            [Header("GRAVITY SETTINGS")]
            [SerializeField] private float _gravityScale = 1f;
            [SerializeField] private float _fallGravityMultiplier = 1.2f;

            [Header("OVERLAP CIRCLE SETTINGS")]
            [SerializeField] private Vector2 _wallCheckOffset = new(0.25f, 0.5f);
            [SerializeField] private float _groundCheckRadius = 0.2f;
            [SerializeField] private float _grabCheckRadius = 0.2f;
            private LayerMask _ignoreLayers;

            private InputHandler _inputHandler;
            private AnimatorManager _animatorManager;
            private BoxCollider2D _boxCollider;
            private Rigidbody2D _rigidbody;
            private Transform _transform;
            private float _verticalVelocity;
            private bool _facingRight;

            public void Init()
            {
                _getters = new();

                int controllerLayer = LayerMask.NameToLayer("Controller");
                int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");
                _ignoreLayers = ~(1 << controllerLayer | 1 << damageColliderLayer);

                _inputHandler = GetComponent<InputHandler>();
                _animatorManager = GetComponent<AnimatorManager>();
                _boxCollider = GetComponent<BoxCollider2D>();
                _rigidbody = GetComponent<Rigidbody2D>();
                _transform = transform;
                _facingRight = true;

                _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                _rigidbody.gravityScale = _gravityScale;
                _rigidbody.isKinematic = false;

                _inputHandler.JumpInputPerformed += Locomotion_JumpInputPerformed;
                _inputHandler.JumpInputCanceled += Locomotion_JumpInputCanceled;
                _inputHandler.DashInputPerformed += Locomotion_DashInputPerformed;
            }

            public void Cleanup()
            {
                _getters = null;

                _inputHandler.JumpInputPerformed -= Locomotion_JumpInputPerformed;
                _inputHandler.JumpInputCanceled -= Locomotion_JumpInputCanceled;
                _inputHandler.DashInputPerformed -= Locomotion_DashInputPerformed;
            }

            public void FixedTick(float deltaTime, float horizontalInput)
            {
                float inputMagnitude = Mathf.Abs(horizontalInput);

                UpdateLocomotionState(deltaTime, horizontalInput, inputMagnitude, _rigidbody.velocity.y);
                HandleHorizontalMovement(horizontalInput, inputMagnitude);
                HandleVerticalMovement();
            }

            private void Locomotion_JumpInputPerformed()
            {
                _lastJumpTime = _jumpBufferTime;
            }

            private void Locomotion_JumpInputCanceled()
            {
                if (_verticalVelocity > 0 && _state == LocomotionState.Jumping)
                {
                    _rigidbody.AddForce((1 - _jumpCutMultiplier) * _verticalVelocity * Vector2.down, ForceMode2D.Impulse);
                }
            }

            private void Locomotion_DashInputPerformed()
            {

            }

            private void UpdateLocomotionState(float deltaTime, float horizontalInput, float inputMagnitude, float verticalVelocity)
            {
                #region Updating Global Fields
                _lastJumpTime -= deltaTime;
                _lastGroundedTime -= deltaTime;

                (float lastGroundedTime, bool grounded, bool hanging) =
                _getters.HandlePhysicsChecks(_boxCollider, _transform, _wallCheckOffset, _ignoreLayers,
                _groundCheckRadius, _grabCheckRadius, _lastGroundedTime, _jumpCoyoteTime);

                _state = _getters.UpdateLocomotionState(verticalVelocity, grounded, hanging);

                _verticalVelocity = verticalVelocity;
                _lastGroundedTime = lastGroundedTime;
                #endregion

                #region Character Turning
                if (!_facingRight && horizontalInput > 0f || _facingRight && horizontalInput < 0f)
                {
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;

                    _facingRight = !_facingRight;
                    transform.localScale = scale;
                }
                #endregion

                #region Animator Calls
                _animatorManager.FixedTick(_state, inputMagnitude);
                #endregion
            }

            private void HandleHorizontalMovement(float horizontalInput, float inputMagnitude)
            {
                #region Movement
                float targetSpeed = horizontalInput * _movementSpeed;
                float speedDifference = targetSpeed - _rigidbody.velocity.x;
                float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01) ? _acceleration : _deceleration;
                float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, _velocityPower) * Mathf.Sign(speedDifference);

                _rigidbody.AddForce(movement * Vector2.right);
                #endregion

                #region Friction
                if (_state == LocomotionState.Grounded && inputMagnitude < 0.01f)
                {
                    float amount = Mathf.Min(Mathf.Abs(_rigidbody.velocity.x), Mathf.Abs(_frictionAmount));
                    amount *= Mathf.Sign(_rigidbody.velocity.x);

                    _rigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
                }
                #endregion
            }

            private void HandleVerticalMovement()
            {
                #region Jumping
                if (_lastJumpTime > 0)
                {
                    _lastJumpTime = 0f;

                    if (_state == LocomotionState.Hanging)
                    {
                        Debug.Log("Wall jump performed");
                    }
                    else if (_state == LocomotionState.Grounded)
                    {
                        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                        _lastGroundedTime = 0f;
                    }
                }
                #endregion

                #region Gravity
                if (_rigidbody.velocity.y < 0)
                {
                    _rigidbody.gravityScale = _gravityScale * _fallGravityMultiplier;
                }
                else
                {
                    _rigidbody.gravityScale = _gravityScale;
                }
                #endregion
            }

            private void OnDrawGizmosSelected()
            {
                if (_boxCollider != null)
                {
                    (Vector2 groundCheckOrigin, Vector2 leftWallCheckOrigin, Vector2 rightWallCheckOrigin) = _getters.GetPhysicsOrigins(_boxCollider, transform, _wallCheckOffset);

                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(groundCheckOrigin, _groundCheckRadius);
                    Gizmos.DrawWireSphere(leftWallCheckOrigin, _grabCheckRadius);
                    Gizmos.DrawWireSphere(rightWallCheckOrigin, _grabCheckRadius);
                }
            }
        }
    }
}