using ShatterStep.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class IceWall : MonoBehaviour
    {
        [Header("AUDIO REFERENCES")]
        [SerializeField] private SoundData _shatterData;

        [Header("SETTINGS")]
        // 28 for just dash, 32 for dash and jump, 38 for dash and walljump
        [SerializeField] private float _minimumVelocityRequirement = 28f;
        [SerializeField] private float _slowedTimeScale = 0.2f;
        [SerializeField] private float _timeSlowDuration = 0.2f;

        private Dictionary<Animator, SpriteRenderer> _childrenDictionary = new();
        private ParticleSystem _particleSystem;
        private BoxCollider2D _boxCollider;
        private AudioSource _audioSource;

        private void Start()
        {
            Animator[] animatorArray = GetComponentsInChildren<Animator>();
            foreach (var animator in animatorArray)
            {
                SpriteRenderer renderer = animator.GetComponent<SpriteRenderer>();
                _childrenDictionary.Add(animator, renderer);
            }

            _particleSystem = GetComponent<ParticleSystem>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out Manager player))
            {
                Data data = player.Data;
                Vector2 previousVelocity = data.PreviousVelocity;

                if (previousVelocity.magnitude > _minimumVelocityRequirement)
                {
                    TimeSystem.Instance.DelayTimeFor(_slowedTimeScale, _timeSlowDuration);
                    data.Rigidbody.velocity = data.PreviousVelocity;
                    StartCoroutine(DestructionCoroutine(3f));
                }
                else
                {
                    float percentage = (previousVelocity.magnitude / _minimumVelocityRequirement) * 3f;
                    int roundedUp = Mathf.CeilToInt(percentage);

                    string animationName = "Crack ";
                    animationName += roundedUp.ToString();

                    foreach (Animator animator  in _childrenDictionary.Keys)
                    {
                        animator.Play(animationName);
                    }
                }
            }
        }

        private IEnumerator DestructionCoroutine(float delay)
        {
            AudioSystem.Instance.PlaySound(_shatterData, _audioSource);

            _particleSystem.Play();
            _boxCollider.enabled = false;

            TurnInvisible();

            yield return new WaitForSeconds(delay);

            Destroy(gameObject);
        }

        private void TurnInvisible()
        {
            foreach (SpriteRenderer renderer in _childrenDictionary.Values)
            {
                Color color = renderer.color;
                color.a = 0;
                renderer.color = color;
            }
        }
    }
}