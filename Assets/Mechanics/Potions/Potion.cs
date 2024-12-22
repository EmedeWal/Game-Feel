using System.Collections;
using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class Potion : PlayerTrigger
    {
        [Header("REFERENCES")]
        [SerializeField] private SoundData _audioData;

        [Header("SETTINGS")]
        [SerializeField] private float _pickupDelay = 3;

        private SpriteRenderer _spriteRender;
        private AudioSource _audioSource;
        private Collider2D _collider;

        protected override void Initialize()
        {
            base.Initialize();

            _spriteRender = GetComponentInChildren<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
            _collider = GetComponent<Collider2D>();

            Data.PlayerRespawn += Potion_PlayerRespawn;
        }

        protected override void Cleanup()
        {
            Data.PlayerRespawn -= Potion_PlayerRespawn;
        }

        protected override void OnPlayerEntered(Manager player)
        {
            player.Data.RefreshAbilities(true);

            StartCoroutine(PotionCoroutine());
        }

        private void Potion_PlayerRespawn()
        {
            StopAllCoroutines();

            _collider.enabled = true;
            _spriteRender.enabled = true;
        }

        private IEnumerator PotionCoroutine()
        {
            AudioSystem.Instance.PlaySound(_audioData, _audioSource);

            _spriteRender.enabled = false;  
            _collider.enabled = false;

            yield return new WaitForSeconds(_pickupDelay);

            _collider.enabled = true;
            _spriteRender.enabled = true;
        }
    }
}