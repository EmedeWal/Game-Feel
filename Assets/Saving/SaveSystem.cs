using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace ShatterStep
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance;
        private static string _saveFilePath;

        public void Initialize()
        {
            Instance = this;
            _saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");

            if (File.Exists(_saveFilePath))
                LoadGame();
        }

        // Save only on main menu quitting
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private void SaveGame()
        {
            AudioSystem audioSystem = AudioSystem.Instance;
            LevelSystem levelSystem = LevelSystem.Instance;

            SaveData saveData = new()
            {
                // Save audio settings
                AudioSettings = new SaveData.AudioSaveData
                {
                    MusicVolume = audioSystem.AudioDictionary[AudioType.Music],
                    SoundVolume = audioSystem.AudioDictionary[AudioType.Sound]
                },

                // Save level data
                Levels = new List<SaveData.LevelSaveData>()
            };

            foreach (var levelData in levelSystem.LevelDataList)
            {
                SaveData.LevelSaveData levelSaveData = new()
                {
                    Name = levelData.Name,
                    Completed = levelData.Completed,
                    Unlocked = levelData.Unlocked,
                    StatEntryList = new()
                };

                foreach (var stat in levelData.StatDictionary)
                {
                    levelSaveData.StatEntryList.Add(new SaveData.LevelSaveData.StatEntry(stat.Key.ToString(), stat.Value));
                }

                saveData.Levels.Add(levelSaveData);
            }

            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(_saveFilePath, json);
        }

        private bool LoadGame()
        {
            AudioSystem audioSystem = AudioSystem.Instance;
            LevelSystem levelSystem = LevelSystem.Instance;

            string json = File.ReadAllText(_saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            // Restore audio settings
            audioSystem.AudioDictionary[AudioType.Music] = saveData.AudioSettings.MusicVolume;
            audioSystem.AudioDictionary[AudioType.Sound] = saveData.AudioSettings.SoundVolume;

            // Restore level data
            var levelDataList = levelSystem.LevelDataList;
            foreach (var levelSaveData in saveData.Levels)
            {
                LevelData levelData = levelDataList.Find(l => l.Name == levelSaveData.Name);
                if (levelData != null)
                {
                    levelData.Completed = levelSaveData.Completed;
                    levelData.Unlocked = levelSaveData.Unlocked;
                    levelData.StatDictionary = new Dictionary<StatType, StatValues>();
                    foreach (var statEntry in levelSaveData.StatEntryList)
                    {
                        if (Enum.TryParse(statEntry.Key, out StatType statType))
                        {
                            levelData.StatDictionary[statType] = statEntry.Value;
                        }
                    }
                }
            }

            return true;
        }
    }
}