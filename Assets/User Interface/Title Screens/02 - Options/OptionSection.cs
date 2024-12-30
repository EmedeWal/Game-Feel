using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class OptionSection : Section
        {
            private AudioSettings[] _audioSettings;

            public override (Section section, Transform[] children) Initialize(Controller controller, int childrenStart = 2)
            {
                _audioSettings = GetComponentsInChildren<AudioSettings>();
                foreach (AudioSettings audioSettings in _audioSettings)
                    audioSettings.Initialize();

                return base.Initialize(controller, childrenStart);
            }

            public override void Cleanup()
            {
                base.Cleanup();

                foreach (AudioSettings audioSettings in _audioSettings)
                    audioSettings.Cleanup();
            }
        }
    }
}