using UnityEngine.SceneManagement;
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
            [SerializeField] private Button _levelSelectButton;
            [SerializeField] private Button _retryButton;

            private StatOverview[] _statList;

            private void Start()
            {
                _levelSelectButton.onClick.AddListener(LoadTitleScreen);
                _retryButton.onClick.AddListener(LoadLevel);

                _levelSelectButton.gameObject.SetActive(false);
                _retryButton.gameObject.SetActive(false);

                _statList = _statContainer.GetComponentsInChildren<StatOverview>();
                foreach (StatOverview stat in _statList) stat.gameObject.SetActive(false);

                StartCoroutine(DisplayStatsWithDelays());
            }

            private void OnDisable()
            {
                _levelSelectButton.onClick.RemoveListener(LoadTitleScreen);
                _retryButton.onClick.RemoveListener(LoadLevel);
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

                _levelSelectButton.gameObject.SetActive(true);
                _retryButton.gameObject.SetActive(true);
            }

            private void LoadTitleScreen()
            {
                SceneManager.LoadScene(0);
            }

            private void LoadLevel()
            {
                SceneManager.LoadScene(LevelSystem.Instance.CurrentLevelData.Name);
            }
        }
    }
}