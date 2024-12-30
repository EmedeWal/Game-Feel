using ShatterStep.Player;
using UnityEngine;

namespace ShatterStep
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelLoader : PlayerTrigger
    {
        [Header("LEVEL DATA")]
        [SerializeField] private LevelData _levelData;

        protected override void OnPlayerEntered(Manager player)
        {
            LevelSystem.Instance.LevelCompleted(_levelData, StatManager.Instance.StatTracker);
            SceneLoader.Instance.LoadNextScene();
        }
    }
}