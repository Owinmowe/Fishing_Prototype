using Mirror;
using UnityEngine;

namespace FishingPrototype.Gameplay.Boat
{
    public class NetworkBoat : NetworkBehaviour, IBoat
    {
        
        [Header("Base Movement Configurations")] 
        [SerializeField] private float accelerationSpeed = 1f;
        [SerializeField] private float rotationSpeed = 1f;
        
        private Rigidbody _rigidbody;
        
        private bool _accelInThisFixedDelta = false;
        private float _accelerationFloat = 0;

        private bool _rotateInThisFixedDelta = false;
        private float _rotationFloat = 0;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            if (_accelInThisFixedDelta)
                Accelerate(transform.right * _accelerationFloat);
            
            if(_rotateInThisFixedDelta)
                Rotate(transform.up * _rotationFloat);
        }
        
        public void ReceiveAcceleration(float accelerationRate)
        {
            if (isLocalPlayer)
                CmdReceiveAcceleration(accelerationRate);
        }

        public void ReceiveRotation(float rotationRate)
        {
            if(isLocalPlayer)
                CmdReceiveRotation(rotationRate);
        }
        
        private void Accelerate(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Acceleration);
            _accelInThisFixedDelta = false;
        }

        private void Rotate(Vector3 torqueForce)
        {
            _rigidbody.AddTorque(torqueForce, ForceMode.Acceleration);
            _rotateInThisFixedDelta = false;
        }

        [Command]
        private void CmdReceiveAcceleration(float accelerationRate)
        {
            _accelerationFloat = Mathf.Clamp(accelerationRate, -1, 1) * accelerationSpeed;
            _accelInThisFixedDelta = true;
        }
        
        [Command]
        private void CmdReceiveRotation(float rotationRate)
        {
            _rotationFloat = Mathf.Clamp(rotationRate, -1, 1) * rotationSpeed;
            _rotateInThisFixedDelta = true;
        }
    }
}
