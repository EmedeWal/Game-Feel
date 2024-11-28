using System.Collections;
using UnityEngine;

namespace ShatterStep
{
    public class TransparentPool : PoolObject
    {
        [Header("SETTINGS")]
        [SerializeField] private float _startDelay = 1.0f;
        [SerializeField] private float _fadeTime = 1.0f;

        private SpriteRenderer _spriteRenderer;
        private float _originalAlpha;

        public override void Init()
        {
            base.Init();

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _originalAlpha = _spriteRenderer.color.a;
        }

        public override void ReuseObject()
        {
            SetAlphaColor(_spriteRenderer, _originalAlpha);

            StopAllCoroutines();
            StartCoroutine(FadeCoroutine());
        }

        private IEnumerator FadeCoroutine()
        {
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