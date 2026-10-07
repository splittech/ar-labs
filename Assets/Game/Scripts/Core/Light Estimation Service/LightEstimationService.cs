using Game.Core.AR;
using R3;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

namespace Game.Core
{
    public class LightEstimationService
    {
        private readonly ARService _ARService;
        private readonly ITickService _tickService;
        private readonly LightEstimationServiceView _view;

        private bool _enabled;

        private DisposableBag _disposableBag;

        private float _defaultIntensity;
        private Color _defaultColor;
        private bool _defaultUseColorTemperature;
        private float _defaultColorTemperature;
        private Quaternion _defaultRotation;
        private AmbientMode _defaultAmbientMode;
        private SphericalHarmonicsL2 _defaultAmbientProbe;
        private float? _targetIntensity;
        private Color? _targetColor;
        private float? _targetColorTemperature;
        private Quaternion? _targetRotation;
        private SphericalHarmonicsL2? _targetAmbientProbe;

        public LightEstimationService(ARService aRService, ITickService tickService, LightEstimationServiceView view)
        {
            _ARService = aRService;
            _tickService = tickService;
            _view = view;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            SaveDefaults();

            _ARService.OnLightEstimated
                .Subscribe(UpdateTargets)
                .AddTo(ref _disposableBag);

            _tickService.OnTick
                .Where(tick => tick.Type == TickType.Update)
                .Subscribe(tick => ApplyTargets(tick.DeltaTime))
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();

            ResetTargets();
            RestoreDefaults();
        }

        private void UpdateTargets(ARLightEstimationData data)
        {
            float? brightness = data.averageBrightness ?? data.averageMainLightBrightness;
            if (brightness.HasValue)
                _targetIntensity = brightness.Value * _view.IntensityMultiplier;

            Color? color = data.colorCorrection ?? data.mainLightColor;
            if (color.HasValue)
                _targetColor = color.Value;

            if (data.averageColorTemperature.HasValue)
                _targetColorTemperature = data.averageColorTemperature.Value;

            if (data.mainLightDirection.HasValue)
                _targetRotation = Quaternion.LookRotation(data.mainLightDirection.Value);

            if (data.ambientSphericalHarmonics.HasValue)
                _targetAmbientProbe = data.ambientSphericalHarmonics.Value;
        }

        private void ApplyTargets(float deltaTime)
        {
            float t = 1f - Mathf.Exp(-_view.SmoothingSharpness * deltaTime);

            if (_targetIntensity.HasValue)
                _view.Intensity = Mathf.Lerp(_view.Intensity, _targetIntensity.Value, t);

            if (_targetColor.HasValue)
                _view.Color = Color.Lerp(_view.Color, _targetColor.Value, t);

            if (_targetColorTemperature.HasValue)
            {
                _view.UseColorTemperature = true;
                _view.ColorTemperature = Mathf.Lerp(_view.ColorTemperature, _targetColorTemperature.Value, t);
            }

            if (_targetRotation.HasValue)
                _view.Rotation = Quaternion.Slerp(_view.Rotation, _targetRotation.Value, t);

            if (_targetAmbientProbe.HasValue)
            {
                RenderSettings.ambientMode = AmbientMode.Skybox;
                RenderSettings.ambientProbe = RenderSettings.ambientProbe * (1f - t) + _targetAmbientProbe.Value * t;
            }
        }

        private void ResetTargets()
        {
            _targetIntensity = null;
            _targetColor = null;
            _targetColorTemperature = null;
            _targetRotation = null;
            _targetAmbientProbe = null;
        }

        private void SaveDefaults()
        {
            _defaultIntensity = _view.Intensity;
            _defaultColor = _view.Color;
            _defaultUseColorTemperature = _view.UseColorTemperature;
            _defaultColorTemperature = _view.ColorTemperature;
            _defaultRotation = _view.Rotation;
            _defaultAmbientMode = RenderSettings.ambientMode;
            _defaultAmbientProbe = RenderSettings.ambientProbe;
        }

        private void RestoreDefaults()
        {
            _view.Intensity = _defaultIntensity;
            _view.Color = _defaultColor;
            _view.UseColorTemperature = _defaultUseColorTemperature;
            _view.ColorTemperature = _defaultColorTemperature;
            _view.Rotation = _defaultRotation;
            RenderSettings.ambientMode = _defaultAmbientMode;
            RenderSettings.ambientProbe = _defaultAmbientProbe;
        }
    }
}
