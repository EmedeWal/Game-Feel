using System.Collections.Generic;
using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class IciclePool : PoolObject
    {
        [Header("SETTINGS")]
        [SerializeField] private float _regrowthDuration = 1f;

        private Dictionary<SpriteRenderer, ParticleSystem> _spriteDictionary;
        private RespawnSystem _respawnSystem;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Vector3 _position;

        public override void Init()
        {
            base.Init();

            _respawnSystem = RespawnSystem.Instance;

            _spriteDictionary = new Dictionary<SpriteRenderer, ParticleSystem>();
            foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            {
                ParticleSystem particleSystem = spriteRenderer.GetComponentInChildren<ParticleSystem>();
                _spriteDictionary.Add(spriteRenderer, particleSystem);
            }

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            _collider = GetComponent<Collider2D>();
        }

        public override void ReuseObject()
        {
            if (!_collider.enabled)
                return;

            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.velocity = Vector2.zero;
            _position = Transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out Manager player))
                _respawnSystem.RespawnPlayer(player.Data);

            SetAlpha(0f);
            PlayParticles();
            Invoke(nameof(Regrow), _regrowthDuration);

            _collider.enabled = false;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void Regrow()
        {
            transform.position = _position;
            _collider.enabled = true;

            SetAlpha(1f);
        }

        private void SetAlpha(float alpha)
        {
            foreach (var renderer in _spriteDictionary.Keys)
            {
                Color color = renderer.color;
                color.a = alpha;
                renderer.color = color;
            }
        }

        private void PlayParticles()
        {
            foreach (var particleSystem in _spriteDictionary.Values)
                particleSystem.Play();
        }
    }
}
