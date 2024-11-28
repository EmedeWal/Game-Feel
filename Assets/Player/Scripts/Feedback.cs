using UnityEngine;

namespace ShatterStep
{
    namespace Player
    {
        public class Feedback
        {
            private Data _d;
            private SpriteRenderer _spriteRenderer;
            private Color _originalColor;
            private Color _targetColor;

            private float _currentTime;
            private float _duration;
            private bool _active;
            private bool _fadingToTarget;

            public Feedback(Data data, SpriteRenderer spriteRenderer)
            {
                _d = data;

                _d.AbilitiesRefreshed += Feedback_AbilitiesRefreshed;
                _d.PlayerRespawned += Feedback_PlayerRespawned;

                _spriteRenderer = spriteRenderer;
                _originalColor = spriteRenderer.color;
            }

            public void Cleanup()
            {
                _d.AbilitiesRefreshed -= Feedback_AbilitiesRefreshed;
                _d.PlayerRespawned -= Feedback_PlayerRespawned;
            }

            public void Tick(float deltaTime)
            {
                if (_active)
                {
                    _currentTime += deltaTime;

                    float transitionTime = _currentTime / _duration;

                    if (_fadingToTarget)
                    {
                        _spriteRenderer.color = Color.Lerp(_originalColor, _targetColor, transitionTime);

                        if (_currentTime >= _duration)
                        {
                            _currentTime = 0;
                            _fadingToTarget = false;
                        }
                    }
                    else
                    {
                        _spriteRenderer.color = Color.Lerp(_targetColor, _originalColor, transitionTime);

                        if (_currentTime >= _duration)
                        {
                            _spriteRenderer.color = _originalColor;
                            _active = false;
                        }
                    }
                }
            }

            private void Activate(Color color, float time, bool instant, bool priority)
            {
                if (_active && !priority) return;

                if (instant)
                {
                    _spriteRenderer.color = color;
                    _fadingToTarget = false;
                    _targetColor = color;
                    _duration = time;
                    _currentTime = 0;
                    _active = true;
                }
                else
                {
                    _targetColor = color;
                    _fadingToTarget = true;
                    _duration = time / 2;
                    _currentTime = 0;
                    _active = true;
                }
            }

            private void Feedback_AbilitiesRefreshed()
            {
                Activate(_d.RefreshColor, _d.RefreshTime, false, false);
            }

            private void Feedback_PlayerRespawned()
            {
                Activate(_d.RespawnColor, _d.RespawnTime, true, true);
            }
        }
    }
}
