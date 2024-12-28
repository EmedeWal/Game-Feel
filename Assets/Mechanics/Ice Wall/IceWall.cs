using System.Collections.Generic;
using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public enum ShatterThreshold
    {
        Dash,
        DashJump,
        DashBounce,
    }

    public class IceWall : MonoBehaviour
    {
        [Header("AUDIO REFERENCES")]
        [SerializeField] private SoundData _crackData;
        [SerializeField] private SoundData _shatterData;

        [Header("COLORS")]
        [SerializeField] private Color[] _intensityColors;

        [Header("SETTINGS")]
        // 28 for just dash, 32 for dash and jump, 38 for dash and walljump
        [SerializeField] private ShatterThreshold _shatterThreshold;
        [SerializeField] private float _slowedTimeScale = 0.2f;
        [SerializeField] private float _timeSlowDuration = 0.2f;

        private Dictionary<Animator, SpriteRenderer> _childrenDictionary = new();
        private ParticleSystem _particleSystem;
        private BoxCollider2D _boxCollider;
        private AudioSource _audioSource;

        private void Start()
        {
            Color color = GetColor();
            Animator[] animatorArray = GetComponentsInChildren<Animator>();
            foreach (var animator in animatorArray)
            {
                SpriteRenderer renderer = animator.GetComponent<SpriteRenderer>();
                renderer.color = color;

                _childrenDictionary.Add(animator, renderer);
            }

            _particleSystem = GetComponent<ParticleSystem>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _audioSource = GetComponent<AudioSource>();

            Data.PlayerRespawn += IceWall_PlayerRespawn;
        }

        private void OnDisable()
        {
            Data.PlayerRespawn -= IceWall_PlayerRespawn;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out Manager player))
            {
                Data data = player.Data;
                Vector2 previousVelocity = data.PreviousVelocity;
                float magnitude = GetIntensity();

                if (previousVelocity.magnitude > magnitude)
                {
                    TimeSystem.Instance.DelayTimeFor(_slowedTimeScale, _timeSlowDuration);
                    data.Rigidbody.velocity = data.PreviousVelocity;

                    Deactivate();
                }
                else
                {
                    AudioSystem.Instance.PlaySound(_crackData, _audioSource);

                    float percentage = previousVelocity.magnitude / magnitude * 3f;
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

        private void IceWall_PlayerRespawn()
        {
            _boxCollider.enabled = true;

            SetAlpha(1);
        }

        private void Deactivate()
        {
            AudioSystem.Instance.PlaySound(_shatterData, _audioSource);

            _particleSystem.Play();
            _boxCollider.enabled = false;

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

        private Color GetColor()
        {
            return _shatterThreshold switch
            {
                ShatterThreshold.Dash => _intensityColors[0],
                ShatterThreshold.DashJump => _intensityColors[1],
                ShatterThreshold.DashBounce => _intensityColors[2],
                _ => _intensityColors[0],
            };
        }

        private int GetIntensity()
        {
            return _shatterThreshold switch
            {
                ShatterThreshold.Dash => 24,
                ShatterThreshold.DashJump => 30,
                ShatterThreshold.DashBounce => 36,
                _ => 24,
            };
        }
    }
}