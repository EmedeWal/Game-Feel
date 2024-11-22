using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Custom
{
    public class CollectibleManager : MonoBehaviour
    {
        private List<CollectibleTracker> _collectibleList;

        [Header("TEXT REFERENCES")]
        [SerializeField] private TextMeshProUGUI _treasureText;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _keyText;

        public void Init()
        {
            _collectibleList = new()
            {
                new CollectibleTracker(CollectibleType.Treasure),
                new CollectibleTracker(CollectibleType.Key),
                new CollectibleTracker(CollectibleType.Coin)
            };

            Collectible[] collectibles = FindObjectsOfType<Collectible>();
            foreach (var collectible in collectibles)
            {
                collectible.Collected += CollectibleManager_Collected;
                collectible.Init();

                CollectibleTracker tracker = _collectibleList.Find(t => t.Type == collectible.Type);
                tracker.Max++;
            }

            UpdateUI();
        }

        private void CollectibleManager_Collected(Collectible collectible)
        {
            CollectibleTracker tracker = _collectibleList.Find(t => t.Type == collectible.Type);
            tracker.Current++;

            collectible.Collected -= CollectibleManager_Collected;
            Destroy(collectible.gameObject);
            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateCount(_treasureText, CollectibleType.Treasure);
            UpdateCount(_keyText, CollectibleType.Key);
            UpdateCount(_coinText, CollectibleType.Coin);
        }

        private void UpdateCount(TextMeshProUGUI text, CollectibleType type)
        {
            CollectibleTracker tracker = _collectibleList.Find(t => t.Type == type);
            text.text = $"{tracker.Current} / {tracker.Max}";
        }
    }
}
