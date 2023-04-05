using UnityEngine;

namespace FishingPrototype.Gameplay.Boat
{
    public class LocalBoat : MonoBehaviour, IBoat
    {

        [Header("Base Movement Configurations")] 
        [SerializeField] private float accelerationSpeed = 1f;
        [SerializeField] private float rotationSpeed = 1f;
        
        private Rigidbody _rigidbody;
        
        private bool accelInThisFixedDelta = false;
        private float accelerationFloat = 0;

        private bool rotateInThisFixedDelta = false;
        private float rotationFloat = 0;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void ReceiveAcceleration(float accelerationRate)
        {
            accelerationFloat = Mathf.Clamp(accelerationRate, -1, 1) * accelerationSpeed;
            accelInThisFixedDelta = true;
            
        }

        public void ReceiveRotation(float rotationRate)
        {
            rotationFloat = Mathf.Clamp(rotationRate, -1, 1) * rotationSpeed;
            rotateInThisFixedDelta = true;
            
        }

        private void FixedUpdate()
        {
            if (accelInThisFixedDelta)
                Accelerate(transform.right * accelerationFloat);
            
            if(rotateInThisFixedDelta)
                Rotate(transform.up * rotationFloat);
        }

        private void Accelerate(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Acceleration);
            accelInThisFixedDelta = false;
        }

        private void Rotate(Vector3 torqueForce)
        {
            _rigidbody.AddTorque(torqueForce, ForceMode.Acceleration);
            rotateInThisFixedDelta = false;
        }
    }
}