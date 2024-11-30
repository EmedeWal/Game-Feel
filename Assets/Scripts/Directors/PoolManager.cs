using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class PoolManager : SingletonBase
    {
        #region Setup
        public static PoolManager Instance { get; private set; }

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

            _poolDictionary = new();
        }
        #endregion

        private Dictionary<int, Queue<ObjectInstance>> _poolDictionary;

        public void CreatePool(PoolObject prefab, int poolSize, Transform parent = null)
        {
            int poolKey = prefab.GetInstanceID();

            if(!_poolDictionary.ContainsKey(poolKey))
            {
                _poolDictionary.Add(poolKey, new Queue<ObjectInstance>());
            }

            for (int i = 0; i < poolSize; i++)
            {
                PoolObject poolObject = Instantiate(prefab);
                poolObject.Init();

                ObjectInstance newObject = new(poolObject, parent);
                _poolDictionary[poolKey].Enqueue(newObject);
            }
        }

        public void ReuseObject(PoolObject prefab, Vector3 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();
            if (_poolDictionary.ContainsKey(poolKey))
            {
                Queue<ObjectInstance> poolQueue = _poolDictionary[poolKey];

                if (poolQueue.Count > 0)
                {
                    ObjectInstance objectInstance = poolQueue.Dequeue();
                    objectInstance.Reuse(position, rotation);
                    poolQueue.Enqueue(objectInstance);
                }
                else
                {
                    Debug.LogWarning($"Pool for {prefab.name} is empty. Consider increasing its size.");
                }
            }
            else
            {
                Debug.LogError($"Pool for prefab {prefab.name} does not exist.");
            }
        }


        public class ObjectInstance
        {
            public PoolObject PoolObject { get; private set; }

            public ObjectInstance(PoolObject objectInstance, Transform parent = null)
            {
                PoolObject = objectInstance;
                PoolObject.GameObject.SetActive(false);

                if (parent)
                {
                    PoolObject.Transform.SetParent(parent, false);
                }
            }

            public void Reuse(Vector3 position, Quaternion rotation)
            {
                if (PoolObject)
                {
                    PoolObject.GameObject.SetActive(true);
                    PoolObject.Transform.position = position;
                    PoolObject.Transform.rotation = rotation;
                    
                    PoolObject.ReuseObject();
                }
                else
                {
                    Debug.LogError("Tried to reuse a pool object that was not set.");
                }
            }
        }
    }
}