using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class VictoryScreenManager : MonoBehaviour
        {
            [Header("SETTINGS")]
            [SerializeField] private float _delayTime = 1f;

            [Header("REFERENCES")]
            [SerializeField] private Transform _statContainer;
            [SerializeField] private Button _mainMenuButton;
            [SerializeField] private Button _retryButton;

            private StatOverview[] _statList;

            private void Start()
            {
                _mainMenuButton.onClick.AddListener(LoadTitleScreen);
                _retryButton.onClick.AddListener(RetryLevel);

                _mainMenuButton.gameObject.SetActive(false);
                _retryButton.gameObject.SetActive(false);

                _statList = _statContainer.GetComponentsInChildren<StatOverview>();
                foreach (StatOverview stat in _statList) stat.gameObject.SetActive(false);

                StartCoroutine(DisplayStatsWithDelays());
            }

            private void OnDisable()
            {
                _mainMenuButton.onClick.RemoveListener(LoadTitleScreen);
                _retryButton.onClick.RemoveListener(RetryLevel);
            }

            private IEnumerator DisplayStatsWithDelays()
            {
                int counter = 0;
                foreach (var stat in LevelSystem.Instance.CurrentLevelData.StatDictionary)
                {
                    _statList[counter].gameObject.SetActive(true);
                    _statList[counter].Initialize(stat.Key, stat.Value);

                    counter++;

                    yield return new WaitForSeconds(_delayTime);
                }

                _mainMenuButton.gameObject.SetActive(true);
                _retryButton.gameObject.SetActive(true);
            }

            private void LoadTitleScreen()
            {
                SceneLoader.Instance.LoadFirstScene();
            }

            private void RetryLevel()
            {
                SceneData[] scenes = LevelSystem.Instance.CurrentLevelData.SceneArray;
                SceneLoader.Instance.EnqueueScenes(scenes);
            }
        }
    }
}