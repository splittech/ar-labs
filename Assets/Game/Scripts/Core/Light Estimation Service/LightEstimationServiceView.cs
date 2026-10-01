using UnityEngine;

namespace Game.Core
{
    public class LightEstimationServiceView : MonoBehaviour
    {
        [SerializeField] private Light _mainLight;

        [Header("Parameters")]
        [SerializeField] private float _intensityMultiplier = 1f;
        [SerializeField] private float _smoothingSharpness = 5f;

        public float IntensityMultiplier => _intensityMultiplier;
        public float SmoothingSharpness => _smoothingSharpness;

        public float Intensity
        {
            get => _mainLight.intensity;
            set => _mainLight.intensity = value;
        }

        public Color Color
        {
            get => _mainLight.color;
            set => _mainLight.color = value;
        }

        public bool UseColorTemperature
        {
            get => _mainLight.useColorTemperature;
            set => _mainLight.useColorTemperature = value;
        }

        public float ColorTemperature
        {
            get => _mainLight.colorTemperature;
            set => _mainLight.colorTemperature = value;
        }

        public Quaternion Rotation
        {
            get => _mainLight.transform.rotation;
            set => _mainLight.transform.rotation = value;
        }
    }
}
