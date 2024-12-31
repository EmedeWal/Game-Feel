using UnityEngine;

namespace ShatterStep
{
    namespace UI
    {
        public class OptionSection : Section
        {
            private SliderSettings[] _sliderSettings;

            public override (Section section, Transform[] children) Initialize(Controller controller, int childrenStart = 2)
            {
                _sliderSettings = GetComponentsInChildren<SliderSettings>();

                foreach (var sliderSetting in _sliderSettings)
                    sliderSetting.Initialize();

                return base.Initialize(controller, childrenStart);
            }

            public override void Cleanup()
            {
                base.Cleanup();

                foreach (var sliderSetting in _sliderSettings)
                    sliderSetting.Cleanup();
            }
        }
    }
}