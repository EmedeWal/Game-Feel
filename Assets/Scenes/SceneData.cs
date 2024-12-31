using ShatterStep.UI;
using UnityEngine;

namespace ShatterStep
{
    [CreateAssetMenu(fileName = "New Scene Data", menuName = "Scriptable Object/Data/Scene")]
    public class SceneData : ScriptableObject
    {
        public string Name;
        public AudioData[] AudioTrackArray;
        public DialogueData DialogueData;
    }
}