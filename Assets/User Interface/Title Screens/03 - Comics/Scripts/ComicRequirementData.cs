using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public abstract class ComicRequirementData : ScriptableObject
        {
            [Header("COMIC SCENE")]
            public SceneData[] ComicScenes;

            public virtual void Initialize(GameObject display) { }

            public abstract bool IsRequirementMet();
        }
    }
}