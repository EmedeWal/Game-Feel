using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class CollectibleManager : MonoBehaviour
    {
        private Dictionary<CollectibleType, CollectibleUI> _collectibleUIDictionary = new();
        private AudioSource _audioSource;
        private AudioSystem _audioSystem;

        public void Setup()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSystem = AudioSystem.Instance;

            Dictionary<CollectibleType, int> collectibleTypeMaxDictionary = new();
            List<CollectibleUI> collectibleUIList = new();

            // Collect all present collectibles and store their max values.
            Collectible[] collectibleArray = FindObjectsOfType<Collectible>();
            foreach (var collectible in collectibleArray)
            {
                collectible.Collected -= CollectibleManager_Collected;
                collectible.Collected += CollectibleManager_Collected;

                CollectibleType type = collectible.Type;
                if (collectibleTypeMaxDictionary.ContainsKey(type))
                {
                    collectibleTypeMaxDictionary[type]++;
                }
                else
                {
                    collectibleTypeMaxDictionary.Add(type, 1);
                }
            }
            CollectibleUI[] collectibleUIArray = FindObjectsOfType<CollectibleUI>();
            foreach (var collectibleUI in collectibleUIArray)
            {
                if (collectibleTypeMaxDictionary.TryGetValue(collectibleUI.Type, out int max))
                {
                    collectibleUI.Setup(max);
                    collectibleUI.UpdateCounter(0);
                    _collectibleUIDictionary.Add(collectibleUI.Type, collectibleUI);
                }
                else
                {
                    Destroy(collectibleUI.gameObject);
                }
            }
        }

        private void CollectibleManager_Collected(Collectible collectible)
        {
            collectible.Collected -= CollectibleManager_Collected;
            _audioSystem.Play(collectible.AudioData, _audioSource);
            Destroy(collectible.gameObject);
            UpdateUI(collectible.Type);
        }

        private void UpdateUI(CollectibleType type, int increment = 1)
        {
            _collectibleUIDictionary[type].UpdateCounter(increment);
        }
    }
}
