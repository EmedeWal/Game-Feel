using System.Collections.Generic;
using ShatterStep.Player;
using System.Collections;
using UnityEngine;

namespace ShatterStep
{
    public class UnstablePlatform : MonoBehaviour
    {
        [Header("SETTINGS")]
        [SerializeField] private float _restorationDelay = 3f;

        [Header("AUDIO REFERENCES")]
        [SerializeField] private SoundData _crackData;
        [SerializeField] private SoundData _spawnData;
        [SerializeField] private SoundData _shatterData;
        [SerializeField] private float _shatterOffset = 0.4f;

        [Header("ANIMATION REFERENCE")]
        [SerializeField] private AnimationClip _animationClip;

        private Dictionary<Animator, SpriteRenderer> _childrenDictionary = new();
        private ParticleSystem _particleSystem;
        private BoxCollider2D _boxCollider;
        private AudioSource _audioSource;
        private int _animationHash;
        private float _animationLength;
        private bool _isCracking;

        private AudioSystem _audioSystem;

        private void Start()
        {
            if (!_animationClip || !_crackData || !_spawnData || !_shatterData)
            {
                Debug.LogError("An Unstable Platform reference was nog assigned!");
            }

            Animator[] animatorArray = GetComponentsInChildren<Animator>();
            foreach (var animator in animatorArray)
            {
                SpriteRenderer renderer = animator.GetComponent<SpriteRenderer>();
                _childrenDictionary.Add(animator, renderer);
            }

            _boxCollider = GetComponentInChildren<BoxCollider2D>();
            _particleSystem = GetComponent<ParticleSystem>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;

            _animationHash = Animator.StringToHash(_animationClip.name);
            _animationLength = _animationClip.length;

            _audioSystem = AudioSystem.Instance;

            Data.PlayerRespawn += UnstablePlatform_PlayerRespawn;
        }

        private void OnDisable()
        {
            Data.PlayerRespawn -= UnstablePlatform_PlayerRespawn;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out Manager player) && !_isCracking)
            {
                StartCoroutine(StateCoroutine());
            }
        }

        private void UnstablePlatform_PlayerRespawn()
        {
            StopAllCoroutines();

            _boxCollider.enabled = true;
            _isCracking = false;
            SetAlpha(1);
        }

        private IEnumerator StateCoroutine()
        {
            foreach (Animator animator in _childrenDictionary.Keys) animator.Play(_animationHash);

            _audioSystem.PlaySound(_crackData, _audioSource);
            _isCracking = true;

            yield return new WaitForSeconds(_animationLength - _shatterOffset);

            _audioSystem.PlaySound(_shatterData, _audioSource);

            yield return new WaitForSeconds(_shatterOffset);

            _boxCollider.enabled = false;
            _particleSystem.Play();
            SetAlpha(0);

            yield return new WaitForSeconds(_restorationDelay);

            _audioSystem.PlaySound(_spawnData, _audioSource);
            _boxCollider.enabled = true;
            _isCracking = false;
            SetAlpha(1);
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