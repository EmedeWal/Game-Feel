using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class LevelSection : Section
        {
            [Header("REFERENCE")]
            [SerializeField] private LevelData _data;

            private Button _playButton;

            public override (Section section, Transform[] children) Initialize(Controller controller, int childrenStart = 2)
            {
                StaticStatUI statUI = GetComponentInChildren<StaticStatUI>();
                if (_data.Completed)
                    statUI.Initialize(_data.StatDictionary);
                else
                    DestroyImmediate(statUI.gameObject);

                // Set the appropriate header color
                Transform secondChild = transform.GetChild(2);
                TextMeshProUGUI text = secondChild.GetComponentInChildren<TextMeshProUGUI>();
                text.text = _data.Name;
                Image image = secondChild.GetComponentInChildren<Image>();
                if (_data.Completed)
                    image.color = _data.UI.CompletedBlock.normalColor;

                return base.Initialize(controller, childrenStart);
            }

            public override void Cleanup()
            {
                base.Cleanup();

                _playButton.onClick.RemoveListener(LoadLevel);
            }

            protected override void InitializeButtons(Button[] buttons)
            {
                base.InitializeButtons(buttons);

                // If the level is completed
                if (_data.Completed)
                {  
                    _OpenButton.colors = _data.UI.CompletedBlock;
                }
                // If the level is locked, cannot open and change colors.
                else if (!_data.Unlocked)
                {
                    _OpenButton.interactable = false;
                    _OpenButton.colors = _data.UI.LockedBlock;
                }
                // If the level is unlocked, but not completed, do nothing

                // Skip the first button, idk know why but GetComponentInChildren[] allocates the button on the object itself [0]
                _playButton = buttons[1];
                _playButton.onClick.AddListener(LoadLevel);
                _playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
            }

            protected override void InitializeText(TextMeshProUGUI text)
            {
                base.InitializeText(text);

                if (!_data.Unlocked)
                    text.color = _data.UI.LockedBlock.normalColor;
            }

            private void LoadLevel() => SceneLoader.Instance.EnqueueScenes(_data.SceneArray);
        }
    }
}