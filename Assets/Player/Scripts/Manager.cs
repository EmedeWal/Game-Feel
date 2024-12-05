using System.Collections;
using UnityEngine;

namespace ShatterStep
{
    namespace Player
    {
        public class Manager : MonoBehaviour
        {
            private InputManager _inputManager;
            private float _deltaTime;

            [Header("SETTINGS")]
            public Data Data;

            private AnimatorManager _animatorManager;
            private Controller _controller;

            private SpriteRenderer _spriteRenderer;
            private Color _originalColor;

            public void Setup()
            {
                _inputManager = InputManager.Instance;

                Data.Init(gameObject, _inputManager);

                _animatorManager = new(GetComponentInChildren<Animator>());
                _controller = new(Data);

                Data.Setup(_controller, _animatorManager);

                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                _originalColor = _spriteRenderer.color;

                Data.PlayerDeath += Manager_PlayerRespawned;
            }

            public void Cleanup()
            {
                _controller.Cleanup();

                _animatorManager = null;
                _controller = null;

                Data.PlayerDeath -= Manager_PlayerRespawned;
            }

            public void Tick(float deltaTime)
            {
                _deltaTime = deltaTime;
            }

            public void FixedTick(float fixedDeltaTime)
            {
                float horizontalInput = _inputManager.HorizontalInput;

                _controller.FixedTick(fixedDeltaTime, horizontalInput);
            }

            private void Manager_PlayerRespawned()
            {
                StartCoroutine(ColorCoroutine(Data.RespawnColor, Data.RespawnTime));
            }

            private IEnumerator ColorCoroutine(Color color, float duration)
            {
                _spriteRenderer.color = color;  
                float time = 0;

                while (time < duration)
                {
                    time += _deltaTime;

                    float transitionTime = time / duration;
                    _spriteRenderer.color = Color.Lerp(color, _originalColor, transitionTime);  

                    yield return null;
                }

                _spriteRenderer.color = _originalColor;
            }
        }
    }
}