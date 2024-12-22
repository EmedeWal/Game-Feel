using UnityEngine;

namespace ShatterStep
{
    namespace TitleScreen
    {
        public class NavigateButton : ElementButton
        {
            [Header("SECTION TO NAVIGATE TO")]
            [SerializeField] private Section _section;

            protected override void OnClick()
            {
                _Controller.ActivateSection(_section);
            }
        }
    }
}