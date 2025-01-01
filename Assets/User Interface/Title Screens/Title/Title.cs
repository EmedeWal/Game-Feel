using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ShatterStep
{
    namespace UI
    {
        public class Title : MonoBehaviour
        {
            private static bool _hasVisited;

            [Header("FLASH SETTINGS")]
            [SerializeField] private float _fadeDuration = 1f;
            [SerializeField] private float _holdDuration = 2f;

            [Header("AUDIO REFERENCE")]
            [SerializeField] private SoundData _windData;

            private AudioSource _audioSource;
            private GameObject _holderObject;
            private Controller _controller;
            private Button _button;
            private Image _flash;

            private void Start()
            {
                _holderObject = transform.GetChild(0).gameObject;
                _controller = FindObjectOfType<Controller>();

                if (!_hasVisited)
                {
                    _audioSource = GetComponent<AudioSource>();
                    _flash = transform.GetChild(1).GetComponent<Image>();
                    _button = _holderObject.GetComponentInChildren<Button>();

                    _button.onClick.AddListener(FirstActivation);

                    _controller.gameObject.SetActive(false);
                }
                else
                { 
                    ActivateController();
                }
            }

            private void OnDisable()
            {
                _controller.Cleanup();
            }

            private void Update()
            {
                if (!_hasVisited && Input.anyKeyDown)
                {
                    FirstActivation();
                }
            }

            private void FirstActivation()
            {
                _hasVisited = true;

                AudioSystem.Instance.PlaySound(_windData, _audioSource);
                _button.onClick.RemoveListener(FirstActivation);

                StartCoroutine(FlashSequence());
            }

            private IEnumerator FlashSequence()
            {
                yield return StartCoroutine(FadeFlash(0f, 1f, _fadeDuration));
                yield return new WaitForSeconds(_holdDuration / 2);

                ActivateController();

                yield return new WaitForSeconds(_holdDuration / 2);
                yield return StartCoroutine(FadeFlash(1f, 0f, _fadeDuration));
            }

            private IEnumerator FadeFlash(float startAlpha, float endAlpha, float duration)
            {
                float elapsed = 0f;
                Color color = _flash.color;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
                    color.a = alpha;
                    _flash.color = color;
                    yield return null;
                }

                color.a = endAlpha;
                _flash.color = color;
            }

            private void ActivateController()
            {
                _controller.gameObject.SetActive(true);
                _controller.Initialize();
                Destroy(_holderObject);
            }
        }
    }
}