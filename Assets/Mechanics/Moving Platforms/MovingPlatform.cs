using ShatterStep.Player;
using System.Collections;
using UnityEngine;

namespace ShatterStep
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("ACTIVATION SETTINGS")]
        [SerializeField] private float _activationDelay = 2f;
        [SerializeField] private bool _activateOnStart = true;
        
        [Header("PATH SETTINGS")]
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private float _movementSpeed = 4f; 
        [SerializeField] private float _waypointPause = 1f;
        [SerializeField] private float _journeyPause = 1f;

        private Rigidbody2D _rb;
        private int _waypointIndex = 0;
        private bool _movingForward = true;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.isKinematic = true;

            if (_waypoints.Length < 2)
            {
                Debug.LogError("MovingPlatform requires at least two waypoints.");
                Destroy(gameObject);
                return;
            }

            if (_activateOnStart)
            {
                StartCoroutine(MovePlatform());
            }

            Data.PlayerRespawn += MovingPlatform_PlayerRespawn;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_activateOnStart && collision.transform.TryGetComponent(out Manager player))
                StartCoroutine(MovePlatform());
        }

        private void MovingPlatform_PlayerRespawn()
        {
            StopAllCoroutines();

            _waypointIndex = 0;
            transform.position = _waypoints[0].position;

            if (_activateOnStart)
            {
                StartCoroutine(MovePlatform());
            }
        }

        private IEnumerator MovePlatform()
        {
            yield return new WaitForSeconds(_activationDelay);

            while (true)
            {
                Transform targetWaypoint = _waypoints[_waypointIndex];
                while (Vector2.Distance(_rb.position, targetWaypoint.position) > 0.1f)
                {
                    Vector2 direction = ((Vector2)targetWaypoint.position - _rb.position).normalized;
                    _rb.MovePosition(_rb.position + _movementSpeed * Time.fixedDeltaTime * direction);
                    yield return new WaitForFixedUpdate();
                }

                _rb.position = targetWaypoint.position;

                if (_movingForward)
                {
                    _waypointIndex++;
                    if (_waypointIndex >= _waypoints.Length)
                    {
                        _movingForward = false;
                        _waypointIndex = _waypoints.Length - 1;
                        yield return new WaitForSeconds(_journeyPause);
                    }
                    else
                    {
                        yield return new WaitForSeconds(_waypointPause);
                    }
                }
                else
                {
                    _waypointIndex--;
                    if (_waypointIndex < 0)
                    {
                        _waypointIndex = 0;
                        _movingForward = true;
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
