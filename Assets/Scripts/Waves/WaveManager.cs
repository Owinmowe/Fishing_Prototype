using System.Collections;
using System.Threading.Tasks;
using FishingPrototype.Utils;
using UnityEngine;

namespace FishingPrototype.Waves
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Wave Configuration")] 
        [SerializeField] private WaveConfiguration waveConfiguration;
        [SerializeField] private float materialChangeSpeed = .5f;

        private float _offset = 100f; //Starting offset so voronoi starts mixed

        private Material _waterMaterial;
        private MeshRenderer _meshRenderer;

        private readonly MaterialPropertiesHelper _materialPropertiesHelper = new();
        private const string OFFSET_PROPERTY_NAME = "_Offset"; 
        private const string AMPLITUDE_PROPERTY_NAME = "_Amplitude"; 
        private const string LENGHT_PROPERTY_NAME = "_Lenght";

        private IEnumerator _changeMaterialIEnumerator;
        
        public static WaveManager Get() => _instance;
        private static WaveManager _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _meshRenderer = GetComponent<MeshRenderer>();
            _waterMaterial = GetComponent<MeshRenderer>().material;
            
            _materialPropertiesHelper.AddProperty(OFFSET_PROPERTY_NAME);
            _materialPropertiesHelper.AddProperty(AMPLITUDE_PROPERTY_NAME);
            _materialPropertiesHelper.AddProperty(LENGHT_PROPERTY_NAME);
        }
        
        private void Update()
        {
            _offset += waveConfiguration.speed * Time.deltaTime;
            _waterMaterial.SetFloat(_materialPropertiesHelper.GetPropertyId(OFFSET_PROPERTY_NAME), _offset);
        }

        private void OnValidate()
        {
            if (!_waterMaterial) return;
            _waterMaterial.SetFloat(_materialPropertiesHelper.GetPropertyId(AMPLITUDE_PROPERTY_NAME), waveConfiguration.amplitude);
            _waterMaterial.SetFloat(_materialPropertiesHelper.GetPropertyId(LENGHT_PROPERTY_NAME), waveConfiguration.lenght);
        }

        public async Task ChangeMaterial(Material waterMaterial, WaveConfiguration newWaveConfiguration = null)
        {
            if (!WaterMaterialIsValid(waterMaterial))
            {
                Debug.LogWarning("Water material provided for Wave Manager is not valid.");
                Debug.LogWarning("Material Change will be ignored.");
                Debug.LogWarning("Material: " + waterMaterial.name);
                return;
            }
            await ChangingMaterial(waterMaterial, newWaveConfiguration);
        }

        private async Task ChangingMaterial(Material waterMaterial, WaveConfiguration newWaveConfiguration = null)
        {
            if (newWaveConfiguration != null)
            {
                waterMaterial.SetFloat(_materialPropertiesHelper.GetPropertyId(AMPLITUDE_PROPERTY_NAME), newWaveConfiguration.amplitude);
                waterMaterial.SetFloat(_materialPropertiesHelper.GetPropertyId(LENGHT_PROPERTY_NAME), newWaveConfiguration.lenght);
            }

            float startingSpeed = waveConfiguration.speed;
            float endSpeed = newWaveConfiguration?.speed ?? startingSpeed;
            
            float t = 0;
            while (t < 1)
            {
                LerpWaveConfigurations(waterMaterial, t, startingSpeed, endSpeed);
                t += Time.deltaTime * materialChangeSpeed;
                await Task.Yield();
            }

            waveConfiguration.speed = endSpeed;
        }

        private void LerpWaveConfigurations(Material waterMaterial, float t, float startingSpeed, float endSpeed)
        {
            _meshRenderer.material.Lerp(_waterMaterial, waterMaterial, t);

            int offsetProperty = _materialPropertiesHelper.GetPropertyId(OFFSET_PROPERTY_NAME);
            _meshRenderer.material.SetFloat(offsetProperty, _offset);

            int amplitudeProperty = _materialPropertiesHelper.GetPropertyId(AMPLITUDE_PROPERTY_NAME);
            waveConfiguration.amplitude = _meshRenderer.material.GetFloat(amplitudeProperty);

            int lenghtProperty = _materialPropertiesHelper.GetPropertyId(LENGHT_PROPERTY_NAME);
            waveConfiguration.lenght = _meshRenderer.material.GetFloat(lenghtProperty);

            waveConfiguration.speed = Mathf.Lerp(startingSpeed, endSpeed, t);
        }

        private bool WaterMaterialIsValid(Material waterMaterial)
        {
            bool hasAllProperties = _materialPropertiesHelper.MaterialHasAllProperties(waterMaterial);
            return waterMaterial != null && hasAllProperties;
        }
        
        public float GetWaveHeight(float x) => waveConfiguration.amplitude * Mathf.Sin((x / waveConfiguration.lenght) + _offset);
    }
}
