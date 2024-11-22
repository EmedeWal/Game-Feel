using UnityEngine;

namespace Custom
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Spikes : MonoBehaviour
    {
        [Header("SETTINGS")]
        [Range(0f, 1f)][SerializeField] private float _verticalMultiplier = 0.8f;
        [SerializeField] private float _force = 28f;

        private void Awake()
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.offset = new Vector2(0, 0.5f);
            boxCollider.size = new Vector2(1.75f, 1);
            boxCollider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Rigidbody2D rigidbody))
            {
                Vector2 direction = Vector2.up * _verticalMultiplier;
                if (rigidbody.transform.localScale.x > 0)
                {
                    direction += Vector2.left;
                }
                else
                {
                    direction += Vector2.right;
                }

                rigidbody.AddForce(direction * _force, ForceMode2D.Impulse);
            }
        }
    }
}