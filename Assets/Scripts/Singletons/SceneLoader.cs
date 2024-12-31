using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using ShatterStep.UI;

namespace ShatterStep
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;

        [Header("REFERENCES")]
        [SerializeField] private List<SceneData> _allScenes;
        private Queue<SceneData> _sceneQueue;
        private SceneData _loadedScene;

        public void Initialize()
        {
            Instance = this;

            _sceneQueue = new();
            _loadedScene = GetActiveScene();
        }

        public void HandleSceneStart()
        {
            AudioSystem.Instance.UpdateMusicTracks(_loadedScene.AudioTrackArray);

            if (_loadedScene.DialogueData != null)
                FindObjectOfType<DialogueManager>().Initialize(_loadedScene.DialogueData);
        }

        public void EnqueueScenes(params SceneData[] scenes)
        {
            _sceneQueue.Clear();

            foreach (var sceneData in scenes)
            {
                _sceneQueue.Enqueue(sceneData);
            }

            LoadNextScene();
        }

        public void LoadScene(SceneData scene)
        {
            _loadedScene = scene;
            SceneManager.LoadScene(scene.Name);
        }

        public void LoadNextScene()
        {
            if (_sceneQueue.Count > 0)
            {
                SceneData nextScene = _sceneQueue.Dequeue();
                LoadScene(nextScene);
            }
            else
            {
                Debug.LogError("Scene queue is empty!");
            }
        }

        public void LoadFirstScene()
        {
            _sceneQueue.Clear();

            SceneData titleScene = GetScene("Title Screen");
            LoadScene(titleScene);
        }

        public SceneData GetScene(string sceneName) => _allScenes.Find(s => s.Name.Equals(sceneName)); 
        public SceneData GetActiveScene() => GetScene(SceneManager.GetActiveScene().name);
    }
}