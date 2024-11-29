using UnityEngine;
using System;

namespace ShatterStep
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

        public event Action TimeScaleReverted;

        public void Setup()
        {
            Time.timeScale = 1;
        }

        public void Tick(float unscaledDeltaTime)
        {
            if (_duration > 0)
            {
                _duration -= unscaledDeltaTime;
            }
            else if (Time.timeScale != 1)
            {
                OnTimeScaleReverted();
                Time.timeScale = 1;
            }
        }

        public void DelayTimeFor(float scale, float duration)
        {
            Time.timeScale = scale; 
            _duration = duration;
        }

        private void OnTimeScaleReverted()
        {
            TimeScaleReverted?.Invoke();
        }
    }
}