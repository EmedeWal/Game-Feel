using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    public class LevelHolder
    {
        private List<GameObject> _childrenList = new();
        private MenuController _menuController;
        private GameObject _parentObject;
        private string _levelName;  

        public LevelHolder(LevelData levelData, MenuController menuController, Transform transform, LevelMenu levelMenu, GameObject gameObject)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                _childrenList.Add(transform.GetChild(i).gameObject);
            }

            _menuController = menuController;
            _parentObject = gameObject;

            _levelName = levelData.Name;

            Button playButton = _childrenList[1].GetComponent<Button>();
            Button backButton = _childrenList[3].GetComponent<Button>();

            GameObject detailObject = _childrenList[2];
            if (!levelData.Completed)
            {
                _childrenList.Remove(detailObject);
                detailObject.SetActive(false);
            }

            playButton.GetComponentInChildren<TextMeshProUGUI>().text = levelData.Completed ? "Retry" : "Play";

            float layoutSpacing = transform.GetComponent<VerticalLayoutGroup>().spacing;
            levelMenu.Length = UIHelpers.GetLengthOfElements(_childrenList, layoutSpacing);

            playButton.onClick.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();

            playButton.onClick.AddListener(Play);
            backButton.onClick.AddListener(Back);
        }

        public void CloseMenu()
        {
            _childrenList[0].SetActive(true);
            for (int i = 1; i < _childrenList.Count; i++)
            {
                _childrenList[i].SetActive(false);
            }
        }

        public void ActivateAll(bool activate)
        {
            for (int i = 0; i < _childrenList.Count; i++)
            {
                _childrenList[i].SetActive(activate);
                _parentObject.SetActive(activate);
            }
        }

        private void Play()
        {
            SceneManager.LoadScene(_levelName);
        }

        private void Back()
        {
            _menuController.OpenLevelSelection();
        }
    }
}