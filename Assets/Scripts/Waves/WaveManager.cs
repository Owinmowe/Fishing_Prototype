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
        }

        private void Update()
        {
            _offset += speed * Time.deltaTime;
        }

        
        public float GetWaveHeight(float x) => amplitude * Mathf.Sin(x / lenght + _offset);
    }
}
