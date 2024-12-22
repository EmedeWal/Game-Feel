using UnityEngine;

namespace ShatterStep
{
    namespace TitleScreen
    {
        public class QuitButton : ElementButton
        {
            protected override void OnClick()
            {
                Application.Quit();
            }
        }
    }
}