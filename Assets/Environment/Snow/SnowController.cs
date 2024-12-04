using ShatterStep.Player;
using UnityEngine;

public class SnowController : MonoBehaviour
{
    [Header("Snow Settings")]
    [SerializeField] private float _rateIncreasePerUnit = 1f;

    private ParticleSystem.EmissionModule _emissionModule;
    private Manager _playerManager;
    private float _startingPlayerY;
    private float _currentEmissionRate;

    public void Initialize(Manager player)
    {
        _playerManager = player;

        _emissionModule = GetComponent<ParticleSystem>().emission;
        _currentEmissionRate = _emissionModule.rateOverTime.constant;
        _startingPlayerY = _playerManager.transform.position.y;
    }

    public void LateTick()
    {
        float playerY = _playerManager.transform.position.y;
        float distanceGained = playerY - _startingPlayerY;

        if (distanceGained > 0)
        {
            float newEmissionRate = _currentEmissionRate + (distanceGained * _rateIncreasePerUnit);
            SetEmissionRate(newEmissionRate);
        }
    }

    private void SetEmissionRate(float newRate)
    {
        var rateOverTime = newRate;
        _emissionModule.rateOverTime = rateOverTime;
    }
}
