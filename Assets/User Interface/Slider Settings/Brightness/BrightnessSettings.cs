namespace ShatterStep
{
    namespace UI
    {
        public class BrightnessSettings : SliderSettings
        {
            private BrightnessOverlay _brightnessOverlay;

            public override void Initialize(float initialValue)
            {
                _brightnessOverlay = BrightnessOverlay.Instance;

                base.Initialize(_brightnessOverlay.CurrentBrightness);
            }

            protected override void ChangeValue(float value) => _brightnessOverlay.SetBrightness(value);
            protected override float GetPreviousValue() => _brightnessOverlay.PreviousBrightness;
            protected override float GetCurrentValue() => _brightnessOverlay.CurrentBrightness;
            protected override float GetDefaultValue() => _brightnessOverlay.DefaultBrightness;
        }
    }
}