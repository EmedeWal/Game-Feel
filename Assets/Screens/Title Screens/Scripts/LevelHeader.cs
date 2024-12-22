using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace TitleScreen
    {
        public class LevelHeader : MonoBehaviour, IElement
        {
            [Header("LEVEL DATA REFERENCE")]
            [SerializeField] private LevelData _data;

            private TextMeshProUGUI _text;
            private Image _image;

            public void Initialize(Controller controller)
            {
                _text = GetComponentInChildren<TextMeshProUGUI>();
                _image = GetComponentInChildren<Image>();

                _text.text = _data.Name;

                _image.color = _data.Completed
                    ? _data.CompletedBlock.normalColor
                    : Color.white;
            }

            public void Cleanup() { }
        }
    }
}