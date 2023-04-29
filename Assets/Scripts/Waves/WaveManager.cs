using FishingPrototype.Utils;
using UnityEngine;

namespace FishingPrototype.Waves
{
    public class WaveManager : MonoBehaviour
    {

        [Header("Wave Configuration")] 
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float lenght = 2f;
        [SerializeField] private float speed = 1f;

        private float _offset;

        private Material _waterMaterial;
        private MeshRenderer _meshRenderer;

        private readonly MaterialPropertiesHelper _materialPropertiesHelper = new();
        private const string OFFSET_PROPERTY_NAME = "_Offset"; 
        private const string AMPLITUDE_PROPERTY_NAME = "_Amplitude"; 
        private const string LENGHT_PROPERTY_NAME = "_Lenght"; 
        
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
            _waterMaterial = GetComponent<MeshRenderer>().sharedMaterial;
            
            _materialPropertiesHelper.AddProperty(OFFSET_PROPERTY_NAME);
            _materialPropertiesHelper.AddProperty(AMPLITUDE_PROPERTY_NAME);
            _materialPropertiesHelper.AddProperty(LENGHT_PROPERTY_NAME);
        }
        
        private void Update()
        {
            _offset += speed * Time.deltaTime;
            _waterMaterial.SetFloat(_materialPropertiesHelper.GetPropertyId(OFFSET_PROPERTY_NAME), _offset);
        }

        private void OnValidate()
        {
            if (!_waterMaterial) return;
            _waterMaterial.SetFloat(_materialPropertiesHelper.GetPropertyId(AMPLITUDE_PROPERTY_NAME), amplitude);
            _waterMaterial.SetFloat(_materialPropertiesHelper.GetPropertyId(LENGHT_PROPERTY_NAME), lenght);
        }

        public void ChangeMaterial(Material waterMaterial)
        {
            if (!WaterMaterialIsValid(waterMaterial)) return;
            _meshRenderer.material = waterMaterial;
            _waterMaterial = waterMaterial;
        }

        private bool WaterMaterialIsValid(Material waterMaterial)
        {
            bool hasAllProperties = _materialPropertiesHelper.MaterialHasAllProperties(waterMaterial);
            return waterMaterial != null && hasAllProperties;
        }
        
        public float GetWaveHeight(float x) => amplitude * Mathf.Sin((x / lenght) + _offset);
    }
}
