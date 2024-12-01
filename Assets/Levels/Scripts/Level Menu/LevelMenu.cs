using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class LevelMenu : MonoBehaviour
        {
            [Header("SETTINGS")]
            public LevelData LevelData;

            private LevelButton _button;
            private LevelHolder _holder;

            private void Start()
            {
                Setup();
            }

            public void Setup()
            {
                _button = new LevelButton(LevelData, transform.GetChild(0).gameObject);
                _holder = new LevelHolder(LevelData, transform.GetChild(1).gameObject);

            }




            //public MenuInfo Info { get; private set; }

            //[Header("SETTINGS")]
            //public LevelData LevelData;

            //private List<GameObject> _childrenList = new();
            //private MenuController _controller;
            //private GameObject _firstChild;
            //private Button _button;

            //public void Setup(MenuController controller)
            //{
            //    for (int i = 0; i < transform.childCount; i++)
            //    {
            //        _childrenList.Add(transform.GetChild(i).gameObject);
            //    }

            //    _firstChild = transform.GetChild(0).gameObject;
            //    _button = _firstChild.GetComponent<Button>();
            //    _controller = controller;

            //    Info = new MenuInfo()
            //    {
            //        Children = transform.childCount - 1,
            //        Spacing = GetComponent<VerticalLayoutGroup>().spacing,
            //        Height = _firstChild.GetComponent<RectTransform>().sizeDelta.y,
            //    };

            //    IElement[] elementArray = GetComponentsInChildren<IElement>();
            //    foreach (IElement element in elementArray)
            //    {
            //        element.Setup(controller, this);
            //    }

            //    _button.onClick.AddListener(OpenMenu);
            //}

            //public void Cleanup()
            //{
            //    IElement[] elementArray = GetComponentsInChildren<IElement>();
            //    foreach (IElement element in elementArray)
            //    {
            //        element.Cleanup();
            //    }

            //    _button.onClick.RemoveAllListeners();
            //}

            //public void Activate(bool activate)
            //{
            //    if (LevelData.Unlocked)
            //    {
            //        foreach (GameObject child in _childrenList)
            //        {
            //            child.SetActive(activate);
            //        }

            //        _firstChild.SetActive(!activate);
            //    }
            //    else
            //    {

            //    }
            //}

            //private void OpenMenu()
            //{
            //    _controller.OpenMenu(this);
            //}
        }
    }
}