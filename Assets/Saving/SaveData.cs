using System.Collections.Generic;
using System;

namespace ShatterStep
{
    [Serializable]
    public class SaveData
    {
        public BrightnessSaveData BrightnessSettings;
        public AudioSaveData AudioSettings;
        public List<LevelSaveData> Levels;


        [Serializable]
        public class BrightnessSaveData
        {
            public float Brightness;
        }

        [Serializable]
        public class AudioSaveData
        {
            public float MusicVolume;
            public float SoundVolume;
        }

        [Serializable]
        public class LevelSaveData
        {
            public string Name;
            public bool Completed;
            public bool Unlocked;
            public List<StatEntry> StatEntryList;

            [Serializable]
            public class StatEntry
            {
                public string Key;
                public StatValues Value;

                public StatEntry(string key, StatValues value)
                {
                    Key = key;
                    Value = value;
                }
            }
        }
    }
}