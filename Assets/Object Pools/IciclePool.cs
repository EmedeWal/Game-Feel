using UnityEngine;

namespace ShatterStep
{
    public class IciclePool : PoolObject
    {
        private SpriteRenderer[] _spriteRendererArray;
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;

        public override void Init()
        {
            base.Init();

            _spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public override void ReuseObject()
        {
            foreach (var renderer in _spriteRendererArray)
            {
                Color color = renderer.color;
                color.a = 1f;
                renderer.color = color;
            }

            _rigidbody.velocity = Vector2.zero;
            _boxCollider.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (var renderer in _spriteRendererArray)
            {
                Color color = renderer.color;
                color.a = 0f;
                renderer.color = color;
            }

            _rigidbody.velocity = Vector2.zero;
            _boxCollider.enabled = false;
        }
    }
}