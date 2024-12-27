using ShatterStep.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class MainSection : Section
        {
            public override (Section section, Transform[] children) Initialize(Controller controller, int childrenStart = 2)
            {
                return base.Initialize(controller, 1);
            }

            protected override void CloseSection()
            {
                string message = "Do you want to quit the game? Progress will be saved.";
                static void action() { Application.Quit(); }
                ConfirmationDialog.Instance.ShowDialog(message, action);
            }
        }
    }
}