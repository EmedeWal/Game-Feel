using UnityEngine;

namespace Custom
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Ice : MonoBehaviour
    {
        [Header("SETTINGS")]
        [SerializeField] private Vector2 _minimalVelocityRequirement;

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Rigidbody2D rigidbody))
            {
                float xVelocity = Mathf.Abs(rigidbody.velocity.x);
                float yVelocity = Mathf.Abs(rigidbody.velocity.y);

                if (xVelocity >= _minimalVelocityRequirement.x && yVelocity >= _minimalVelocityRequirement.y)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
