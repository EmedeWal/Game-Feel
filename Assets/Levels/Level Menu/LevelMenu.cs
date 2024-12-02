using UnityEngine;

namespace ShatterStep
{
    public class LevelMenu : MonoBehaviour
    {
        [Header("SETTINGS")]
        public LevelData LevelData;

        public LevelButton Button { get; private set; }
        public LevelHolder Holder { get; private set; }

        public void Setup(MenuController controller)
        {
            Holder = new LevelHolder(LevelData, gameObject, controller, transform);
            Button = new LevelButton(LevelData, Holder, controller, transform);
        }
    }
}