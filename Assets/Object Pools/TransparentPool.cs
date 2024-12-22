using System.Collections;
using UnityEngine;

namespace ShatterStep
{
    public class TransparentPool : PoolObject
    {
        private AudioSystem _audioSystem;

        [Header("VISUAL SETTINGS")]
        [SerializeField] private float _startDelay = 1.0f;
        [SerializeField] private float _fadeTime = 1.0f;

        private SpriteRenderer _spriteRenderer;
        private float _originalAlpha;

        [Header("AUDIO SETTINGS")]
        [SerializeField] private SoundData _audioData;
        private AudioSource _audioSource;

        public override void Init()
        {
            base.Init();

            if (_audioData == null)
            {
                Debug.LogError("Transparent Pool audio data was not assigned!");
            }

            _audioSystem = AudioSystem.Instance;

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _originalAlpha = _spriteRenderer.color.a;

            _audioSource = GetComponentInChildren<AudioSource>();
        }

        public override void ReuseObject()
        {
            SetAlphaColor(_spriteRenderer, _originalAlpha);

            StopAllCoroutines();
            StartCoroutine(FadeCoroutine());
        }

        private IEnumerator FadeCoroutine()
        {
            _audioSystem.PlaySound(_audioData, _audioSource);

            yield return new WaitForSeconds(_startDelay);

            float elapsedTime = 0f;
            while (elapsedTime < _fadeTime)
            {
                float alpha = Mathf.Lerp(_originalAlpha, 0f, elapsedTime / _fadeTime);
                SetAlphaColor(_spriteRenderer, alpha);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SetAlphaColor(_spriteRenderer, 0);
        }

        private void SetAlphaColor(SpriteRenderer sprite, float alpha)
        {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
        }
    }
}