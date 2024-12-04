using System.Collections;
using UnityEngine;

namespace ShatterStep
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Path Settings")]
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private float _movementSpeed = 2f; 
        [SerializeField] private float _waypointPause = 1f;
        [SerializeField] private float _journeyPause = 2f;

        private Rigidbody2D _rb;
        private int _wapointIndex = 0;
        private bool _movingForward = true;

        private void Start()
        {
            _rb = GetComponentInChildren<Rigidbody2D>();
            _rb.isKinematic = true;

            if (_waypoints.Length < 2)
            {
                Debug.LogError("MovingPlatform requires at least two waypoints.");
                Destroy(gameObject);
                return;
            }

            StartCoroutine(MovePlatform());
        }

        private IEnumerator MovePlatform()
        {
            while (true)
            {
                Transform targetWaypoint = _waypoints[_wapointIndex];
                while (Vector2.Distance(_rb.position, targetWaypoint.position) > 0.1f)
                {
                    Vector2 direction = ((Vector2)targetWaypoint.position - _rb.position).normalized;
                    _rb.MovePosition(_rb.position + _movementSpeed * Time.fixedDeltaTime * direction);
                    yield return new WaitForFixedUpdate();
                }

                _rb.position = targetWaypoint.position;

                if (_movingForward)
                {
                    _wapointIndex++;
                    if (_wapointIndex >= _waypoints.Length)
                    {
                        _movingForward = false;
                        _wapointIndex = _waypoints.Length - 1;
                        yield return new WaitForSeconds(_journeyPause);
                    }
                    else
                    {
                        yield return new WaitForSeconds(_waypointPause);
                    }
                }
                else
                {
                    _wapointIndex--;
                    if (_wapointIndex < 0)
                    {
                        _movingForward = true;
                        _wapointIndex = 0;
                        yield return new WaitForSeconds(_journeyPause);
                    }
                    else
                    {
                        yield return new WaitForSeconds(_waypointPause);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (_waypoints != null && _waypoints.Length > 1)
            {
                for (int i = 0; i < _waypoints.Length - 1; i++)
                {
                    if (_waypoints[i] != null && _waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
                    }
                }
            }
        }
    }
}
