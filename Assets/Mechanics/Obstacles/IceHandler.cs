using ShatterStep.Utilities;
using ShatterStep.Player;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    public class IceHandler : MonoBehaviour
    {
        [Header("AUDIO SETTINGS")]
        [SerializeField] private AudioData _audioData;

        [Header("TIME SETTINGS")]
        [SerializeField] private float _timeScale = 0.2f;
        [SerializeField] private float _timeDuration = 0.5f;

        [Header("COLOR SETTINGS")]
        [SerializeField] private Color[] _colorArray;

        [Header("FEEDBACK SETTINGS")]
        [SerializeField] private TextMeshProUGUI _velocityText;
        [SerializeField] private float _fadeDuration = 1f;

        [Header("VELOCITY SETTINGS")]
        [SerializeField] private float _xRequirement = 33f;

        private Trigger[] _triggers;
        private AudioSystem _audioSystem;
        private TimeSystem _timeSystem;
        private float _elapsedTime;

        public void Setup()
        {
            _triggers = GetComponentsInChildren<Trigger>();
            foreach (Trigger ice in _triggers)
            {
                ice.PlayerEntered += Ice_PlayerEntered;
                ice.Init();
            }

            _audioSystem = AudioSystem.Instance;
            _timeSystem = TimeSystem.Instance;

            SetAlphaColor(_velocityText, 0);
        }

        public void Tick(float deltaTime)
        {
            if (_velocityText.color.a > 0)
            {
                _elapsedTime += deltaTime;

                float alpha = Mathf.Lerp(1f, 0f, _elapsedTime / _fadeDuration);
                SetAlphaColor(_velocityText, alpha);
            }
        }

        public void Cleanup()
        {
            foreach (Trigger ice in _triggers)
            {
                ice.PlayerEntered -= Ice_PlayerEntered;
            }
        }

        private void Ice_PlayerEntered(Manager player, Trigger trigger)
        {
            float xVelocity = Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x);
            if (xVelocity > 0f)
            {
                float rawPercentage = xVelocity / _xRequirement * 100;
                int percentage = Mathf.RoundToInt(rawPercentage);

                if (xVelocity >= _xRequirement)
                {
                    //_audioSystem.Play(_audioData);
                    _timeSystem.DelayTimeFor(_timeScale, _timeDuration);

                    Destroy(trigger.gameObject);
                }

                ShowFeedbackText(percentage, trigger.transform.position);
            }
        }

        private void ShowFeedbackText(float percentage, Vector3 position)
        {
            _elapsedTime = 0;

            _velocityText.gameObject.SetActive(true);
            _velocityText.transform.position = position;

            _velocityText.color = ColorHelpers.GetColorBasedOnPercentage(_colorArray, 100, percentage);
            _velocityText.text = $"{percentage}%";

            SetAlphaColor(_velocityText, 255);
        }

        private void SetAlphaColor(TextMeshProUGUI text, float alpha)
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }
}
