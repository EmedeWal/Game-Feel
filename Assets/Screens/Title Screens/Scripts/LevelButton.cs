using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace TitleScreen
    {
        public class LevelButton : NavigateButton
        {
            [Header("LEVEL DATA REFERENCE")]
            [SerializeField] private LevelData _data;

            private TextMeshProUGUI _text;
            private Image _image;

            public override void Initialize(Controller controller)
            {
                _Controller = controller;
                _Button = GetComponent<Button>();

                _text = GetComponentInChildren<TextMeshProUGUI>();
                _image = GetComponentInChildren<Image>();
                
                _Button.targetGraphic = _image;

                _text.text = _data.Name;

                if (_data.Completed)
                {
                    _text.color = Color.white;
                    _Button.colors = _data.CompletedBlock;
                    _Button.onClick.AddListener(OnClick);
                }
                else if (_data.Unlocked)
                {
                    _text.color = Color.white;
                    _Button.onClick.AddListener(OnClick);
                }
                else
                {
                    _text.color = _data.LockedBlock.normalColor;
                    _Button.colors = _data.LockedBlock;
                }
            }

            public override void Cleanup()
            {
                base.Cleanup();
            }

            protected override void OnClick()
            {
                base.OnClick();
            }
        }
    }
}