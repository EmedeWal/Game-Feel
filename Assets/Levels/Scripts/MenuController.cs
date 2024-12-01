using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class MenuController : MonoBehaviour
        {
            //[Header("REFERENCES")]
            //[SerializeField] private GameObject _pressAnyKeyObject;

            //[Header("SETTINGS")]
            //[SerializeField] private float expansionDuration = 1f;

            //private Dictionary<LevelMenu, GameObject> _menuDictionary = new();
            //private RectTransform _backgroundRect;
            //private MenuInfo _info;
            //private float _standardOffset;

            //private void Awake()
            //{
            //    Cursor.lockState = CursorLockMode.None;
            //    Cursor.visible = true;

            //    _standardOffset = Mathf.Abs(transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y);
            //    _backgroundRect = GetComponent<RectTransform>();

            //    LevelMenu[] menuArray = transform.GetChild(0).GetComponentsInChildren<LevelMenu>();
            //    foreach (LevelMenu menu in menuArray)
            //    {
            //        menu.Setup(this);
            //        _menuDictionary.Add(menu, menu.gameObject);
            //    }

            //    Transform holder = transform.GetChild(0);
            //    _info = new MenuInfo()
            //    {
            //        Children = holder.childCount,
            //        Height = holder.GetComponent<RectTransform>().sizeDelta.y,
            //        Spacing = holder.GetComponent<VerticalLayoutGroup>().spacing,
            //    };

            //    DisableMenu();
            //    StartCoroutine(InputCoroutine());
            //}

            //private void OnDisable()
            //{
            //    foreach (LevelMenu menu in _menuDictionary.Keys)
            //    {
            //        menu.Cleanup();
            //    }
            //}

            //public void OpenMenu(LevelMenu menu = null)
            //{
            //    MenuInfo menuInfo = menu != null ? menu.Info : _info;
            //    float targetHeight = Utilities.GetLength(menuInfo);

            //    StartCoroutine(OpenMenuCoroutine(targetHeight + _standardOffset, menu));
            //}

            //private void SetHeight(RectTransform rectTransform, float height)
            //{
            //    Vector2 sizeDelta = rectTransform.sizeDelta;
            //    sizeDelta.y = height;
            //    rectTransform.sizeDelta = sizeDelta;
            //}

            //private void DisableMenu()
            //{
            //    foreach (GameObject menu in _menuDictionary.Values)
            //    {
            //        menu.SetActive(false);
            //    }
            //}

            //private void Enable(LevelMenu menu)
            //{
            //    if (menu != null)
            //    {
            //        _menuDictionary[menu].SetActive(true);
            //        menu.Activate(true);
            //    }
            //    else
            //    {
            //        foreach (LevelMenu localMenu in _menuDictionary.Keys)
            //        {
            //            _menuDictionary[localMenu].SetActive(true);
            //            localMenu.Activate(false);            
            //        }
            //    }
            //}

            //private IEnumerator OpenMenuCoroutine(float targetHeight, LevelMenu menu = null)
            //{
            //    DisableMenu();

            //    Vector2 initialSize = _backgroundRect.sizeDelta;
            //    float elapsedTime = 0f;

            //    while (elapsedTime < expansionDuration)
            //    {
            //        elapsedTime += Time.deltaTime;
            //        float progress = Mathf.Clamp01(elapsedTime / expansionDuration);

            //        float currentHeight = Mathf.Lerp(initialSize.y, targetHeight, progress);
            //        SetHeight(_backgroundRect, currentHeight);

            //        yield return null;
            //    }

            //    SetHeight(_backgroundRect, targetHeight);
            //    Enable(menu);
            //}

            //private IEnumerator InputCoroutine()
            //{
            //    while (_pressAnyKeyObject)
            //    {
            //        yield return null;

            //        if (Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            //        {
            //            Destroy(_pressAnyKeyObject);
            //            OpenMenu();
            //        }
            //    }
            //}
        }
    }
}
