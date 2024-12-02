using System.Collections;
using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class Potion : PlayerTrigger
    {
        private AudioSystem _audioSystem;

        [Header("REFERENCES")]
        [SerializeField] private AudioData _audioData;

        [Header("SETTINGS")]
        [SerializeField] private float _pickupDelay = 3;

        private SpriteRenderer _spriteRender;
        private AudioSource _audioSource;
        private Collider2D _collider;

        protected override void Initialize()
        {
            _audioSystem = AudioSystem.Instance;

            _spriteRender = GetComponentInChildren<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
            _collider = GetComponent<Collider2D>();

            _collider.isTrigger = true;
        }

        protected override void OnPlayerEntered(Manager player)
        {
            player.Data.RefreshAbilities();

            StartCoroutine(PotionCoroutine());
        }

        private IEnumerator PotionCoroutine()
        {
            _audioSystem.PlayAudio(_audioData, _audioSource);

            _spriteRender.enabled = false;  
            _collider.enabled = false;

            yield return new WaitForSeconds(_pickupDelay);

            _collider.enabled = true;
            _spriteRender.enabled = true;
        }
    }
}