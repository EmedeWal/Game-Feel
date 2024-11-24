using UnityEngine;

namespace Custom
{
    namespace Player
    {
        public class Feedback
        {
            private SpriteRenderer _spriteRenderer;
            private Color _originalColor;
            private Color _dashColor;

            private float _cooldownProgress;
            private float _cooldownTime;
            private bool _onCooldown;

            public Feedback(Controller controller, SpriteRenderer spriteRenderer, Color dashColor)
            {
                controller.Data.DashCooldownStarted += Feedback_DashCooldownStarted;
                controller.Data.DashCooldownFinished += Feedback_DashCooldownFinished;

                _spriteRenderer = spriteRenderer;
                _originalColor = spriteRenderer.color;
                _dashColor = dashColor;
            }

            public void Cleanup(Controller controller)
            {
                controller.Data.DashCooldownStarted -= Feedback_DashCooldownStarted;
                controller.Data.DashCooldownFinished -= Feedback_DashCooldownFinished;
            }

            public void Tick(float deltaTime)
            {
                if (_onCooldown)
                {
                    _cooldownProgress += deltaTime / _cooldownTime;
                    _spriteRenderer.color = Color.Lerp(_dashColor, _originalColor, _cooldownProgress);
                }
            }

            private void Feedback_DashCooldownStarted(float cooldown)
            {
                _spriteRenderer.color = _dashColor;
                _cooldownTime = cooldown;
                _cooldownProgress = 0;
                _onCooldown = true;
            }

            private void Feedback_DashCooldownFinished()
            {
                _spriteRenderer.color = _originalColor;
                _onCooldown = false;
            }
        }
    }
}