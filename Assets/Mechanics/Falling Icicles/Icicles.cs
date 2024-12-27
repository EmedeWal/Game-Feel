using System.Collections.Generic;
using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class Icicles : PlayerTrigger
    {
        [Header("SETTINGS")]
        [SerializeField] private float _regrowthDuration = 1f;

        private Dictionary<SpriteRenderer, ParticleSystem> _spriteDictionary;
        private ColliderEvent _colliderEvent;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private GameObject _childObject;
        private Transform _childTransform;
        private Transform _transform;
        private Vector3 _position;

        protected override void Initialize()
        {
            base.Initialize();

            _colliderEvent = GetComponentInChildren<ColliderEvent>();

            _childTransform = transform.GetChild(0);
            _childObject = _childTransform.gameObject;
            
            _spriteDictionary = new Dictionary<SpriteRenderer, ParticleSystem>();
            foreach (var spriteRenderer in _childObject.GetComponentsInChildren<SpriteRenderer>())
            {
                ParticleSystem particleSystem = spriteRenderer.GetComponentInChildren<ParticleSystem>();
                _spriteDictionary.Add(spriteRenderer, particleSystem);
            }

            _rigidbody = _childObject.GetComponent<Rigidbody2D>();
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            _collider = _childObject.GetComponent<Collider2D>();
            _collider.isTrigger = false;

            _transform = transform;
            _position = _transform.position;

            Data.PlayerRespawn += Icicle_PlayerRespawn;
            _colliderEvent.CollisionEnter += Icicle_CollisionEnter;
        }

        protected override void Cleanup()
        {
            Data.PlayerRespawn -= Icicle_PlayerRespawn;
            _colliderEvent.CollisionEnter -= Icicle_CollisionEnter;
        }

        protected override void OnPlayerEntered(Manager player)
        {
            if (!_collider.enabled)
                return;

            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.velocity = Vector2.zero;
        }

        private void Icicle_PlayerRespawn()
        {
            CancelInvoke();

            Regrow();
        }

        private void Icicle_CollisionEnter(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out Manager player))
                RespawnSystem.Instance.RespawnPlayer(player.Data);

            SetAlpha(0f);
            PlayParticles();
            Invoke(nameof(Regrow), _regrowthDuration);

            _collider.enabled = false;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void Regrow()
        {
            _childTransform.position = _position;
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