using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    public class LevelButton
    {
        public Button Button { get; private set; }

        private MenuController _menuController;
        private LevelMenu _levelMenu;

        private TextMeshProUGUI _text;
        private Image _image;

        public LevelButton(LevelData levelData, MenuController menuController, Transform transform, LevelMenu levelMenu)
        {
            _levelMenu = levelMenu;
            _menuController = menuController;

            transform = transform.GetChild(0);
            _text = transform.GetComponentInChildren<TextMeshProUGUI>();
            _image = transform.GetComponentInChildren<Image>();
            Button = transform.GetComponent<Button>();

            Button.onClick.RemoveAllListeners();
            Button.targetGraphic = _image;

            _text.text = levelData.Name;

            if (levelData.Completed)
            {
                _text.color = Color.white;  
                Button.colors = levelData.CompletedBlock;
                Button.onClick.AddListener(OpenLevelDetails);
            }
            else if (levelData.Unlocked)
            {
                _text.color = Color.white;
                Button.colors = levelData.UnlockedBlock;
                Button.onClick.AddListener(OpenLevelDetails);
            }
            else
            {
                _text.color = levelData.LockedBlock.normalColor;
                Button.colors = levelData.LockedBlock;
            }
        }

        private void OpenLevelDetails()
        {
            _menuController.OpenLevelMenu(_levelMenu);
        }
    }
}