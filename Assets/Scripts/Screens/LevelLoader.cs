using UnityEngine.SceneManagement;
using Custom.Player;
using UnityEngine;

namespace Custom
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelLoader : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Manager player))
            {
                int sceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(sceneIndex + 1);
            }
        }
    }
}
