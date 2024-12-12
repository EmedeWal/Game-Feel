using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class Icicles : PlayerTrigger
    {
        [Header("REFERENCE")]
        [SerializeField] private PoolObject _iciclePool;

        protected override void Initialize()
        {
            base.Initialize();

            PoolManager.Instance.CreatePool(_iciclePool, 1);
        }

        protected override void OnPlayerEntered(Manager player)
        {
            PoolManager.Instance.ReuseObject(_iciclePool, transform.position, Quaternion.identity);
        }
    }
}