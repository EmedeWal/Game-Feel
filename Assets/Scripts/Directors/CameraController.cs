using Cinemachine;
using UnityEngine;

namespace ShatterStep
{
    public class CameraController : MonoBehaviour
    {
        private void Awake()
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = playerTransform;
            virtualCamera.LookAt = playerTransform;

            PolygonCollider2D boundsCollider = GameObject.FindGameObjectWithTag("Bounds").GetComponent<PolygonCollider2D>();
            CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();
            confiner.m_BoundingShape2D = boundsCollider;

            Destroy(this);
        }
    }
}