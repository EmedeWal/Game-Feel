using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        [CreateAssetMenu(fileName = "Dialogue 01", menuName = "Scriptable Object/Data/Dialogue")]
        public class DialogueData : ScriptableObject
        {
            [Header("ART")]
            public Sprite BackgroundSprite;
            
            [Header("DIALOGUE")]
            [TextArea] public string[] Dialogue;
        }
    }
}