using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class Icicles : PlayerTrigger
    {
        [Header("REFERENCE")]
        [SerializeField] private PoolObject _iciclePool;

        private PoolManager _poolManager;
        private Vector3 _position;

        protected override void Initialize()
        {
            base.Initialize();

            _poolManager = PoolManager.Instance;
            _position = transform.position;

            _poolManager.CreatePool(_iciclePool, 1);
            _poolManager.SetupObject(_iciclePool, _position, Quaternion.identity);
        }

        protected override void OnPlayerEntered(Manager player)
        {
            _poolManager.ReuseObject(_iciclePool, _position, Quaternion.identity);
        }
    }
}