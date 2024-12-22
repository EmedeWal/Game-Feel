using UnityEngine;

namespace ShatterStep
{
    public class LevelMenu : MonoBehaviour
    {
        [HideInInspector] public float Length;

        [Header("SETTINGS")]
        public LevelData LevelData;

        //private LevelButton _Button;
        private LevelHolder _holder;
    
        public void Initialize(MenuController controller)
        {
            _holder = new LevelHolder(LevelData, controller, transform, this, gameObject);
            //_Button = new LevelButton(LevelData, controller, transform, this);

            if (LevelData.Completed)
            {
                StaticStatUI statUI = GetComponentInChildren<StaticStatUI>();
                statUI.Initialize(LevelData.StatDictionary);
            }
        }

        public void CloseMenu()
        {
            _holder.CloseMenu();
            //_Button._Button.interactable = true;
        }

        public void ActivateAll(bool activate)
        {
            _holder.ActivateAll(activate);
            //_Button._Button.interactable = false;
        }
    }
}