using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    public class LevelButton
    {
        private TextMeshProUGUI _text;
        private Button _button;
        private Image _image;

        public LevelButton(LevelData levelData, GameObject gameObject)
        {
            _text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            _image = gameObject.GetComponentInChildren<Image>();
            _button = gameObject.GetComponent<Button>();

            _button.targetGraphic = _image;

            if (levelData.Completed)
            {
                _text.color = Color.white;  
                _button.colors = levelData.CompletedBlock;
            }
            else if (levelData.Unlocked)
            {
                _text.color = Color.white;
                _button.colors = levelData.UnlockedBlock;
            }
            else
            {
                _text.color = levelData.LockedBlock.normalColor;
                _button.colors = levelData.LockedBlock;
            }
        }
    }
}