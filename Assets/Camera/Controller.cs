using UnityEngine;

namespace Custom
{
    namespace Camera
    {
        public class Controller : MonoBehaviour
        {
            [Header("SETTINGS")]
            [SerializeField] private float _followSpeed = 5f;

            private Transform _followTarget;
            private Transform _transform;

            private void Awake()
            {
                Init();
            }

            private void FixedUpdate()
            {
                FixedTick();
            }

            public void Init()
            {
                _followTarget = GameObject.FindGameObjectWithTag("Player").transform;
                _transform = transform;
            }

            public void FixedTick()
            {
                _transform.position = Vector2.Lerp(_transform.position, _followTarget.position, _followSpeed);
            }
        }
    }
}