using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace ShatterStep
{
    public class IcePool : PoolObject
    {
        [Header("AUDIO REFERENCES")]
        [SerializeField] private AudioData _spawnData;
        [SerializeField] private AudioData _shatterData;
        [SerializeField] private float _shatterOffset = 0.4f;

        [Header("ANIMATION REFERENCE")]
        [SerializeField] private AnimationClip _animationClip;

        private Dictionary<Animator, SpriteRenderer> _childrenDictionary = new();
        private ParticleSystem _particleSystem;
        private BoxCollider2D _boxCollider;
        private AudioSource _audioSource;
        private int _animationHash;
        private float _animationLength;

        private AudioSystem _audioSystem;

        public override void Init()
        {
            base.Init();

            if (!_animationClip || !_spawnData || !_shatterData)
            {
                Debug.LogError("An Ice Pool reference was nog assigned!");
            }

            Animator[] animatorArray = GetComponentsInChildren<Animator>();
            foreach (var animator in animatorArray)
            {
                SpriteRenderer renderer = animator.GetComponent<SpriteRenderer>();
                _childrenDictionary.Add(animator, renderer);
            }

            _particleSystem = GetComponent<ParticleSystem>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _boxCollider.enabled = false;

            _animationHash = Animator.StringToHash(_animationClip.name);
            _animationLength = _animationClip.length;

            _audioSystem = AudioSystem.Instance;
        }

        public override void ReuseObject()
        {
            StartCoroutine(StateCoroutine());
        }

        private IEnumerator StateCoroutine()
        {
            foreach (Animator animator in _childrenDictionary.Keys) animator.Play(_animationHash);

            _audioSystem.PlaySound(_spawnData, _audioSource);
            _boxCollider.enabled = true;
            SetAlpha(1);

            yield return new WaitForSeconds(_animationLength - _shatterOffset);

            _audioSystem.PlaySound(_shatterData, _audioSource);

            yield return new WaitForSeconds(_shatterOffset);

            _boxCollider.enabled = false;
            _particleSystem.Play();
            SetAlpha(0);
        }

        private void SetAlpha(float alpha)
        {
            foreach (SpriteRenderer renderer in _childrenDictionary.Values)
            {
                Color color = renderer.color;
                color.a = alpha;
                renderer.color = color;
            }
        }
    }
}