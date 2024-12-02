using UnityEngine;

namespace ShatterStep
{
    public class TitleScreenManager : MonoBehaviour
    {
        private void Awake()
        {
            FindObjectOfType<LevelTracker>().Initialize();
            FindObjectOfType<MenuController>().Initialize();
        }
    }
}