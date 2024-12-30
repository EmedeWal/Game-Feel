using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class ConfirmationDialog : MonoBehaviour
        {
            public static ConfirmationDialog Instance;

            public bool Active => _holderObject.activeSelf;

            private GameObject _holderObject;
            private Image _background;

            private TextMeshProUGUI _text;
            private Button _confirmButton;
            private Button _cancelButton;

            public void Initialize()
            {
                Instance = this;

                _background = transform.GetChild(0).GetComponent<Image>();

                Transform holderTransform = transform.GetChild(1);
                int childCount = holderTransform.childCount;
                _holderObject = holderTransform.gameObject;
                _text = holderTransform.GetComponentInChildren<TextMeshProUGUI>();
                _cancelButton = holderTransform.GetChild(childCount - 1).GetComponent<Button>();
                _confirmButton = holderTransform.GetChild(childCount - 2).GetComponent<Button>();

                HideDialog();
            }

            public void ShowDialog(string message, Action confirmAction)
            {
                _text.text = message;

                EnableMessage(true);

                _cancelButton.onClick.AddListener(HideDialog);
                _confirmButton.onClick.AddListener(() =>
                {
                    confirmAction?.Invoke();
                    HideDialog();
                });
            }

            public void HideDialog()
            {
                EnableMessage(false);

                _cancelButton.onClick.RemoveAllListeners();
                _confirmButton.onClick.RemoveAllListeners();
            }

            private void EnableMessage(bool enable)
            {
                _background.enabled = enable;
                _holderObject.SetActive(enable);
            }
        }
    }
}