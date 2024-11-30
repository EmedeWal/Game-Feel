using UnityEngine;

namespace ShatterStep
{
	public abstract class PoolObject : MonoBehaviour 
	{
		public GameObject GameObject { get; private set; }
		public Transform Transform { get; private set; }

		public virtual void Init()
		{
			GameObject = gameObject;
			Transform = transform;
		}

        public abstract void ReuseObject();
	}
}