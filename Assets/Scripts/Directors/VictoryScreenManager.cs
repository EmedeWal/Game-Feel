using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    // Make a test button to retry this.
    // Make a render texture to render the player idle on the right
    public class VictoryScreenManager : MonoBehaviour
    {
        [Header("SETTINGS")]
        [SerializeField] private float _delayTime = 1f;

        [Header("REFERENCES")]
        [SerializeField] private StatOverview _statOverviewPrefab;
        [SerializeField] private Transform _prefabContainer;
        [SerializeField] private Button _button;

        private LevelData _currentLevel;

        private void Start()
        {
            _currentLevel = LevelSystem.Instance.CurrentLevelData;

            _button.onClick.AddListener(LoadTitleScreen);
            _button.gameObject.SetActive(false);

            StartCoroutine(DisplayStatsWithDelays());
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(LoadTitleScreen);
        }

        private IEnumerator DisplayStatsWithDelays()
        {
            foreach (var stat in _currentLevel.StatDictionary)
            {
                StatOverview statOverview = Instantiate(_statOverviewPrefab, _prefabContainer);
                statOverview.Initialize(stat.Key, stat.Value);
                yield return new WaitForSeconds(_delayTime);
            }

            _button.gameObject.SetActive(true);
        }

        private void LoadTitleScreen()
        {
            SceneManager.LoadScene(0);
        }

        public void Reset()
        {
            for (int i = 0; i < _prefabContainer.childCount; i++)
            {
                Destroy(_prefabContainer.GetChild(i).gameObject);
            }

            _currentLevel = LevelSystem.Instance.CurrentLevelData;

            _button.onClick.AddListener(LoadTitleScreen);
            _button.gameObject.SetActive(false);

            StartCoroutine(DisplayStatsWithDelays());
        }
    }
}