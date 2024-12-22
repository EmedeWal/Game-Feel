using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    public class MenuController : MonoBehaviour
    {
        private List<LevelMenu> _levelMenuList = new();
        private RectTransform _backgroundRect;
        private float _controllerLength;
        private float _standardOffset;

        private void Start()
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
                menu.Initialize(this);
                _levelMenuList.Add(menu);
            }

            OpenLevelSelection();
        }

        public void OpenLevelSelection()
        {
            SetBackdropSize(_controllerLength);

            foreach (LevelMenu menu in _levelMenuList)
            {
                menu.ActivateAll(true);
                menu.CloseMenu();
            }
        }

        public void OpenLevelMenu(LevelMenu menu)
        {
            SetBackdropSize(menu.Length);

            foreach (LevelMenu localMenu in _levelMenuList)
            {
                localMenu.ActivateAll(false);
            }

            menu.ActivateAll(true);
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