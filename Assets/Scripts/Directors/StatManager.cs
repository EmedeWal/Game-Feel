using System.Collections.Generic;
using System.Collections;
using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    public class StatManager : SingletonBase
    {
        #region Singleton
        public static StatManager Instance { get; private set; }

        public override void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        public StatTracker StatTracker {  get; private set; }    

        private AudioSource _audioSource;
        private AudioSystem _audioSystem;
        private StatUI _statUI;
        private Data _player;

        public void Setup(Manager player)
        {
            _player = player.Data;

            _audioSource = GetComponent<AudioSource>();
            _audioSystem = AudioSystem.Instance;

            Dictionary<StatType, int> collectibleMax = FindCollectibles();
            Dictionary<StatType, int> maxValues = new()
            {
                { StatType.Time, 3600 },
                { StatType.Death, 999 },
                { StatType.Coin, collectibleMax[StatType.Coin] },
                { StatType.Key, collectibleMax[StatType.Key] },
            };
            StatTracker = new(maxValues);

            _statUI = FindObjectOfType<StatUI>();

            StartCoroutine(TimeCoroutine());
            _statUI.Setup(StatTracker);
            _player.PlayerDeath += StatisticsManager_PlayerDeath;
        }

        public void Cleanup()
        {
            StopAllCoroutines();
            _statUI.Cleanup();
            _player.PlayerDeath -= StatisticsManager_PlayerDeath;
        }

        private void StatisticsManager_Collected(Collectible collectible)
        {
            StatTracker.IncrementStat(collectible.Type, 1);
            _audioSystem.PlayAudio(collectible.AudioData, _audioSource);
            collectible.Collected -= StatisticsManager_Collected;
            Destroy(collectible.gameObject);
        }

        private void StatisticsManager_PlayerDeath()
        {
            StatTracker.IncrementStat(StatType.Death, 1);
        }

        private Dictionary<StatType, int> FindCollectibles()
        {
            Dictionary<StatType, int> collectibleMaxDictionary = new();
            Collectible[] collectibleArray = FindObjectsOfType<Collectible>();
            foreach (var collectible in collectibleArray)
            {
                collectible.Collected -= StatisticsManager_Collected;
                collectible.Collected += StatisticsManager_Collected;

                StatType type = collectible.Type;
                if (collectibleMaxDictionary.ContainsKey(type))
                {
                    collectibleMaxDictionary[type]++;
                }
                else
                {
                    collectibleMaxDictionary.Add(type, 1);
                }
            }

            return collectibleMaxDictionary;
        }

        private IEnumerator TimeCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                StatTracker.IncrementStat(StatType.Time, 1);
            }
        }
    }
}
