using System;
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
        private int _offsetMaterialPropertyId;
        private int _amplitudeMaterialPropertyId;
        private int _lenghtMaterialPropertyId;

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
            
            _waterMaterial = GetComponent<MeshRenderer>().sharedMaterial;
            _offsetMaterialPropertyId = Shader.PropertyToID("_Offset");
            _amplitudeMaterialPropertyId = Shader.PropertyToID("_Amplitude");
            _lenghtMaterialPropertyId = Shader.PropertyToID("_Lenght");
        }
        
        private void Update()
        {
            _offset += speed * Time.deltaTime;
            _waterMaterial.SetFloat(_offsetMaterialPropertyId, _offset);
        }

        private void OnValidate()
        {
            if (!_waterMaterial) return;
            _waterMaterial.SetFloat(_offsetMaterialPropertyId, _offset);
            _waterMaterial.SetFloat(_amplitudeMaterialPropertyId, amplitude);
            _waterMaterial.SetFloat(_lenghtMaterialPropertyId, lenght);
        }

        public void UpdateMaterialReference()
        {
            _waterMaterial = GetComponent<MeshRenderer>().sharedMaterial;
        }
        
        public float GetWaveHeight(float x) => amplitude * Mathf.Sin((x / lenght) + _offset);

    }
}
