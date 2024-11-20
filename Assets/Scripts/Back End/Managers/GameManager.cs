using Custom.Player;
using UnityEngine;

namespace Custom
{
    public class GameManager : MonoBehaviour
    {
        private Controller _player;

        private void Awake()
        {
            SingletonBase[] singletons = FindObjectsByType<SingletonBase>(FindObjectsSortMode.None);
            foreach (var singleton in singletons) singleton.Init();

            _player = FindObjectOfType<Controller>();

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