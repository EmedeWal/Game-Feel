using UnityEngine;

namespace Custom
{
    namespace Player
    {
        [RequireComponent(typeof(CircleCollider2D))]
        [RequireComponent(typeof(Rigidbody2D))]
        public class Locomotion : MonoBehaviour
        {
            [Header("FRICTION SETTINGS")]
            [SerializeField] private float _frictionAmount = 0.25f;

            [Header("SPEED SETTINGS")]
            [SerializeField] private float _movementSpeed = 9f;
            [SerializeField] private float _velocityPower = 0.95f;
            [SerializeField] private float _acceleration = 16f;
            [SerializeField] private float _deceleration = 13f;

            [Header("JUMP SETTINGS")]
            [Range(0, 1)][SerializeField] private float _jumpCutMultiplier = 0.4f;
            [SerializeField] private float _jumpCoyoteTime = 0.15f;
            [SerializeField] private float _jumpBufferTime = 0.1f;
            [SerializeField] private float _jumpForce = 13f;
            private float _lastGroundedTime = 0f;
            private float _lastJumpTime = 0f;
            private bool _isJumping = false;

            [Header("GRAVITY SETTINGS")]
            [SerializeField] private float _gravityScale = 1f;
            [SerializeField] private float _fallGravityMultiplier = 2f;
            [SerializeField] private Vector2 _groundCheckSize = new(0.4f, 0.1f);
             private LayerMask _ignoreLayers;

            private CircleCollider2D _circleCollider2D;
            private Rigidbody2D _rigidbody2D;

            public void Init()
            {
                int controllerLayer = LayerMask.NameToLayer("Controller");
                int damageColliderLayer = LayerMask.NameToLayer("DamageCollider");
                _ignoreLayers = ~(1 << controllerLayer | 1 << damageColliderLayer);

                _circleCollider2D = GetComponent<CircleCollider2D>();

                _rigidbody2D = GetComponent<Rigidbody2D>();
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                _rigidbody2D.gravityScale = _gravityScale;
                _rigidbody2D.isKinematic = false;
            }

            public void FixedTick(float deltaTime, float horizontalInput, bool jumpInput)
            {
                GroundCheck();
                TickTimers(deltaTime);
                HandleHorizontalMovement(horizontalInput);
                HandleVerticalMovement(jumpInput);
                HandleFriction(horizontalInput);
                HandleGravity();
            }

            private void HandleHorizontalMovement(float horizontalInput)
            {
                float targetSpeed = horizontalInput * _movementSpeed;
                float speedDifference = targetSpeed - _rigidbody2D.velocity.x;
                float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01) ? _acceleration : _deceleration;
                float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, _velocityPower) * Mathf.Sign(speedDifference);

                _rigidbody2D.AddForce(movement * Vector2.right);
            }

            private void HandleVerticalMovement(bool jumpInput)
            {
                if (jumpInput)
                {
                    _lastJumpTime = _jumpBufferTime;

                    if (_lastGroundedTime > 0 && _lastJumpTime > 0 && !_isJumping)
                    {
                        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                        _isJumping = true;
                        _lastGroundedTime = 0f;
                        _lastJumpTime = 0f;
                    }
                }
                else
                {
                    if (_rigidbody2D.velocity.y > 0 && _isJumping)
                    {
                        _rigidbody2D.AddForce((1 - _jumpCutMultiplier) * _rigidbody2D.velocity.y * Vector2.down, ForceMode2D.Impulse);
                        _isJumping = false;
                    }
                }
            }

            private void HandleGravity()
            {
                if (_rigidbody2D.velocity.y < 0)
                {
                    _rigidbody2D.gravityScale = _gravityScale * _fallGravityMultiplier;
                }
                else
                {
                    _rigidbody2D.gravityScale = _gravityScale;
                }
            }

            private void HandleFriction(float inputMagnitude)
            {
                if (_lastGroundedTime > 0 && Mathf.Abs(inputMagnitude) < 0.01f)
                {
                    float amount = Mathf.Min(Mathf.Abs(_rigidbody2D.velocity.x), Mathf.Abs(_frictionAmount));
                    amount *= Mathf.Sign(_rigidbody2D.velocity.x);

                    _rigidbody2D.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
                }
            }

            private void TickTimers(float deltaTime)
            {
                _lastGroundedTime -= deltaTime;
                _lastJumpTime -= deltaTime;
            }

            private bool GroundCheck()
            {
                Vector2 position = _circleCollider2D.bounds.center;
                position.y -= _circleCollider2D.bounds.extents.y;

                if (Physics2D.OverlapBox(position, _groundCheckSize, 0, _ignoreLayers))
                {
                    _lastGroundedTime = _jumpCoyoteTime;
                    _isJumping = false;
                    return true;
                }
                return false;
            }

            private void OnDrawGizmosSelected()
            {
                if (_circleCollider2D != null)
                {
                    Gizmos.color = Color.red;
                    Vector2 position = _circleCollider2D.bounds.center;
                    position.y -= _circleCollider2D.bounds.extents.y;
                    Gizmos.DrawWireCube(position, _groundCheckSize);
                }
            }
        }
    }
}