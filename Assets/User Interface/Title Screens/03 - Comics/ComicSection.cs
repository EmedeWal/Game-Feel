using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class ComicSection : Section
        {
            [Header("REFERENCES")]
            [SerializeField] private ComicRequirementData[] _comicRequirements;
            [SerializeField] private UserInterfaceData _UI;

            private readonly List<SceneData> _comicScenes = new();

            private RequirementState _requirementState;
            private Button _openButton;

            public override (Section section, Transform[] children) Initialize(Controller controller, int childrenStart = 2)
            {
                CollectComics();

                // Determine comic requirement state
                if (_comicScenes.Count == GetMaxComicCount())
                    _requirementState = RequirementState.Completed;
                else if (_comicScenes.Count == 0)
                    _requirementState = RequirementState.Locked;
                else
                    _requirementState = RequirementState.Progress;

                // set the header (not the object itself) correct
                Transform thirdChild = transform.GetChild(2);
                TextMeshProUGUI headerText = thirdChild.GetComponentInChildren<TextMeshProUGUI>();
                headerText.text = gameObject.name;
                Image image = thirdChild.GetComponentInChildren<Image>();

                // Set the header image color correct
                if (_requirementState is RequirementState.Completed)
                    image.color = _UI.CompletedBlock.normalColor;

                return base.Initialize(controller, childrenStart);
            }

            public override void Cleanup()
            {
                base.Cleanup();

                _openButton.onClick.RemoveListener(LoadComics);
            }

            protected override void InitializeButtons(Button[] buttons)
            {
                base.InitializeButtons(buttons);

                // Skip the first button, idk know why but GetComponentInChildren[] allocates the button on the object itself [0]
                _openButton = buttons[1];
                _openButton.onClick.AddListener(LoadComics);

                TextMeshProUGUI openButtonText = _openButton.GetComponentInChildren<TextMeshProUGUI>();
                openButtonText.text = "Read";

                if (_requirementState is RequirementState.Completed)
                {
                    _openButton.colors = _UI.CompletedBlock;
                }
                else if (_requirementState is RequirementState.Locked)
                {
                    _openButton.interactable = false;
                    _openButton.colors = _UI.LockedBlock;
                    openButtonText.color = _UI.LockedBlock.normalColor;
                }

                // Make sure the comic set ends on the title screen
                SceneData titleScene = SceneLoader.Instance.GetScene("Title Screen");
                _comicScenes.Add(titleScene);
            }

            private void LoadComics() => SceneLoader.Instance.EnqueueScenes(_comicScenes.ToArray());

            private void CollectComics()
            {
                foreach (var comicRequirement in _comicRequirements)
                    if (comicRequirement.IsRequirementMet())
                        foreach (var scene in comicRequirement.ComicScenes)
                            _comicScenes.Add(scene);
            }

            private int GetMaxComicCount()
            {
                int maxComics = 0;
                foreach (var comicRequirement in _comicRequirements)
                    foreach (var scene in comicRequirement.ComicScenes)
                        maxComics++;

                return maxComics;
            }
        }
    }
}
