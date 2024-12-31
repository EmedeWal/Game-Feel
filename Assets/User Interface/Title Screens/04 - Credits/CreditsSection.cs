using UnityEngine.UI;

namespace ShatterStep
{
    namespace UI
    {
        public class Credits : Section
        {
            public override void Cleanup()
            {
                // No quit button here
                _OpenButton.onClick.RemoveListener(OpenSection);
            }

            protected override void InitializeButtons(Button[] buttons)
            {
                // No quit button here
                _OpenButton = GetComponent<Button>();

                _OpenButton.onClick.AddListener(OpenSection);
            }

            protected override void OpenSection()
            {
                // Load next scene in build index, which is the credit scene
                SceneLoader sceneLoader = SceneLoader.Instance;
                SceneData creditScene = sceneLoader.GetScene("Credit Screen");
                sceneLoader.LoadScene(creditScene);
            }
        }
    }
}