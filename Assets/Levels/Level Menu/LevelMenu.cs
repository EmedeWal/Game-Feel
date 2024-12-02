using UnityEngine;

namespace ShatterStep
{
    public class LevelMenu : MonoBehaviour
    {
        [HideInInspector] public float Length;

        [Header("SETTINGS")]
        public LevelData LevelData;

        private LevelButton _button;
        private LevelHolder _holder;
    
        public void Initialize(MenuController controller)
        {
            _holder = new LevelHolder(LevelData, controller, transform, this, gameObject);
            _button = new LevelButton(LevelData, controller, transform, this);

            if (LevelData.Completed)
            {
                StaticStatUI statUI = GetComponentInChildren<StaticStatUI>();
                statUI.Initialize(LevelData.StatDictionary);
            }
        }

        public void CloseMenu()
        {
            _holder.CloseMenu();
            _button.Button.interactable = true;
        }

        public void ActivateAll(bool activate)
        {
            _holder.ActivateAll(activate);
            _button.Button.interactable = false;
        }
    }
}