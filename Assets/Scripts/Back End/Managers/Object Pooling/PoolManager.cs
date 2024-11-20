using System.Collections.Generic;
using UnityEngine;

namespace Custom
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

        public PoolObject ReuseObject(PoolObject prefab, Vector3 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();
            if (_poolDictionary.ContainsKey(poolKey))
            {
                ObjectInstance objectToReuse = _poolDictionary[poolKey].Dequeue();
                _poolDictionary[poolKey].Enqueue(objectToReuse);

                objectToReuse.Reuse(position, rotation);

                return objectToReuse.PoolObject;
            }
            return null;
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
