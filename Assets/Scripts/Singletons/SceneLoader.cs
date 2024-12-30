using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;

        private Queue<string> _sceneQueue;

        public void Initialize()
        {
            Instance = this;

            _sceneQueue = new Queue<string>();
        }

        public void EnqueueScenes(params string[] sceneNames)
        {
            _sceneQueue.Clear();

            foreach (string sceneName in sceneNames)
            {
                _sceneQueue.Enqueue(sceneName);
            }

            LoadNextScene();
        }

        public void LoadNextScene()
        {
            if (_sceneQueue.Count > 0)
            {
                string nextScene = _sceneQueue.Dequeue();
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                Debug.LogError("Scene queue is empty!");
            }
        }

        public void LoadFirstScene()
        {
            _sceneQueue.Clear();

            SceneManager.LoadScene(0);
        }
    }
}
