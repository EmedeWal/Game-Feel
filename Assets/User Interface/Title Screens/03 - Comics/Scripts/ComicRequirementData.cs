using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public abstract class ComicRequirementData : ScriptableObject
        {
            [Header("COMIC SCENE")]
            public SceneData[] ComicScenes;

            public abstract bool IsRequirementMet();
        }
    }
}