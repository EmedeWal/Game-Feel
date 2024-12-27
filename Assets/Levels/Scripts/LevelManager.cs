using System.Collections.Generic;
using UnityEngine;

namespace ShatterStep
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton
        public static LevelManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        private List<LevelData> _levelDataList;
    }
}