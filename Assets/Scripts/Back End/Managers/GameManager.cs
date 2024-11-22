using Custom.Player;
using UnityEngine;

namespace Custom
{
    public class GameManager : MonoBehaviour
    {
        private CollectibleManager _collectibleManager;
        private Controller _player;

        private void Awake()
        {
            SingletonBase[] singletons = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
            foreach (var singleton in singletons) singleton.Init();

            _collectibleManager = FindObjectOfType<CollectibleManager>();
            _player = FindObjectOfType<Controller>();

            _collectibleManager.Init();
            _player.Init();
        }

        private void OnDisable()
        {
            _player.Cleanup();
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;

            _player.FixedTick(fixedDeltaTime);
        }
    }
}