using UnityEngine;

namespace Custom
{
    public class TimeSystem : SingletonBase
    {
        #region Singleton
        public static TimeSystem Instance { get; private set; }

        public override void Init()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        private float _duration = 0;

        public void Setup()
        {
            Time.timeScale = 1;
        }

        public void UnscaledTick(float unscaledDeltaTime)
        {
            if (_duration > 0)
            {
                _duration -= unscaledDeltaTime;
            }
            else if (Time.timeScale != 1)
            {
                Time.timeScale = 1;
            }
        }

        public void DelayTimeFor(float scale, float duration)
        {
            Time.timeScale = scale; 
            _duration = duration;
        }
    }
}