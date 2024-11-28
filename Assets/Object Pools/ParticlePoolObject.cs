using UnityEngine;

namespace ShatterStep
{
	public class ParticlePoolObject : PoolObject
	{
		ParticleSystem _particle;

        public override void Init()
        {
            base.Init();

            _particle = GetComponent<ParticleSystem>();
        }

        public override void ReuseObject()
		{
            _particle.Play();
        }
	}
}
