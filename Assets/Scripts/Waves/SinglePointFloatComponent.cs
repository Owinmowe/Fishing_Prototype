using UnityEngine;

namespace FishingPrototype.Waves
{
    public class SinglePointFloatComponent : MonoBehaviour
    {
        [Header("Float Configurations")]
        [SerializeField] private float depthBeforeSubmerged = 1f;
        [SerializeField] private float displacementAmount = 3f;
        
        private Vector3 _buoyancyForce = Vector3.zero;
        private Rigidbody _rigidbody = null;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float waveHeight = WaveManager.Get().GetWaveHeight(transform.position.x);
            
            if (transform.position.y > waveHeight) return;

            float yPosition = waveHeight - transform.position.y;
            float displacementMultiplier = Mathf.Clamp01(yPosition / depthBeforeSubmerged) * displacementAmount;

            _buoyancyForce.y = Mathf.Abs(Physics.gravity.y) * displacementMultiplier;
            _rigidbody.AddForce(_buoyancyForce, ForceMode.Acceleration);
        }
    }
}

