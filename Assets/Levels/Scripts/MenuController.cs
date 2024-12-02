using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    public class MenuController : MonoBehaviour
    {
        private List<LevelHolder> _levelHolderList = new();
        private RectTransform _backgroundRect;
        private float _controllerLength;
        private float _standardOffset;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Transform firstChild = transform.GetChild(0);
            _backgroundRect = GetComponent<RectTransform>();
            _standardOffset = Mathf.Abs(firstChild.GetComponent<RectTransform>().anchoredPosition.y);
            _controllerLength = UIHelpers.GetLengthOfChildren(firstChild, firstChild.GetComponent<VerticalLayoutGroup>().spacing);

            LevelMenu[] menuArray = firstChild.GetComponentsInChildren<LevelMenu>();
            foreach (LevelMenu menu in menuArray)
            {
                menu.Setup(this);
                _levelHolderList.Add(menu.Holder);
            }

            OpenLevelSelection();
        }

        public void OpenLevelSelection()
        {
            SetBackdropSize(_controllerLength);

            foreach (LevelHolder holder in _levelHolderList)
            {
                holder.ActivateAll(true);
                holder.CloseMenu();
            }
        }

        public void OpenLevelMenu(LevelHolder h)
        {
            SetBackdropSize(h.Length);

            foreach (LevelHolder holder in _levelHolderList)
            {
                holder.ActivateAll(false);
            }

            h.ActivateAll(true);
        }

        private void SetBackdropSize(float targetHeight)
        {
            targetHeight += _standardOffset;
            Vector2 sizeDelta = _backgroundRect.sizeDelta;
            sizeDelta.y = targetHeight;
            _backgroundRect.sizeDelta = sizeDelta;
        }
    }
}