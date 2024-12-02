using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    public class LevelButton
    {
        private MenuController _menuController;
        private LevelHolder _levelHolder;

        private TextMeshProUGUI _text;
        private Button _button;
        private Image _image;

        public LevelButton(LevelData levelData, LevelHolder levelHolder, MenuController menuController, Transform transform)
        {
            _levelHolder = levelHolder;
            _menuController = menuController;

            transform = transform.GetChild(0);
            _text = transform.GetComponentInChildren<TextMeshProUGUI>();
            _image = transform.GetComponentInChildren<Image>();
            _button = transform.GetComponent<Button>();

            _button.onClick.RemoveAllListeners();
            _button.targetGraphic = _image;

            _text.text = levelData.Name;

            if (levelData.Completed)
            {
                _text.color = Color.white;  
                _button.colors = levelData.CompletedBlock;
                _button.onClick.AddListener(OpenLevelDetails);
            }
            else if (levelData.Unlocked)
            {
                _text.color = Color.white;
                _button.colors = levelData.UnlockedBlock;
                _button.onClick.AddListener(OpenLevelDetails);
            }
            else
            {
                _text.color = levelData.LockedBlock.normalColor;
                _button.colors = levelData.LockedBlock;
            }
        }

        private void OpenLevelDetails()
        {
            _menuController.OpenLevelMenu(_levelHolder);
        }
    }
}