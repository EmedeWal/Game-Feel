using System.Collections;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class HighScore : MonoBehaviour
        {
            [Header("Settings")]
            [SerializeField] private Color[] _colorArray;
            [SerializeField] private float _colorLerpSpeed = 3f;

            private TextMeshProUGUI _highScoreText;

            public void Initialize(bool show)
            {
                _highScoreText = GetComponent<TextMeshProUGUI>();

                if (show)
                {
                    _highScoreText.text = " NEW HIGHSCORE!";
                    _highScoreText.enabled = true;

                    StartCoroutine(LerpColors());
                }
                else
                {
                    StopAllCoroutines();

                    _highScoreText.text = "";
                    _highScoreText.enabled = false;
                }
            }

            private IEnumerator LerpColors()
            {
                int colorIndex = 0;

                while (true)
                {
                    Color startColor = _colorArray[colorIndex];
                    Color endColor = _colorArray[(colorIndex + 1) % _colorArray.Length];
                    float elapsedTime = 0f;

                    while (elapsedTime < 1f)
                    {
                        _highScoreText.color = Color.Lerp(startColor, endColor, elapsedTime);
                        elapsedTime += Time.deltaTime * _colorLerpSpeed;
                        yield return null;
                    }

                    colorIndex = (colorIndex + 1) % _colorArray.Length;
                }
            }
        }
    }
}
