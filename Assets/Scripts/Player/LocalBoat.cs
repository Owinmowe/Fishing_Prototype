using System;
using FishingPrototype.Gameplay.FishingSpot;
using UnityEngine;

namespace FishingPrototype.Gameplay.Boat
{
    public class LocalBoat : MonoBehaviour, IBoat
    {
        public event Action<IFishingSpot> OnFishingActionStarted;
        public event Action OnFishingActionFailed;
        public event Action OnFishingActionCanceled;
        public GameObject BaseGameObject => gameObject;

        [Header("Base Movement Configurations")] 
        [SerializeField] private float accelerationSpeed = 1f;
        [SerializeField] private float rotationSpeed = 1f;

        [Header("Fishing Configurations")]
        [SerializeField] private float fishingDistance = 5f;
        [SerializeField] private LayerMask fishingLayerMask;
        
        private Rigidbody _rigidbody;
        
        private bool _accelInThisFixedDelta = false;
        private float _accelerationFloat = 0;

        private bool _rotateInThisFixedDelta = false;
        private float _rotationFloat = 0;

        private const int MAX_FISHING_COLLIDERS_SIZE = 20;
        private IFishingSpot _currentFishingSpot;
        private readonly Collider[] _fishingColliders = new Collider[MAX_FISHING_COLLIDERS_SIZE];
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            IBoat.onLocalBoatSet?.Invoke(this);
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
            if (_currentFishingSpot != null) return;
            
            _accelerationFloat = Mathf.Clamp(accelerationRate, -1, 1) * accelerationSpeed;
            _accelInThisFixedDelta = true;
            
        }

        public void ReceiveRotation(float rotationRate)
        {
            if (_currentFishingSpot != null) return;
            
            _rotationFloat = Mathf.Clamp(rotationRate, -1, 1) * rotationSpeed;
            _rotateInThisFixedDelta = true;
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
        
        public void TryFishing()
        {
            if (_currentFishingSpot != null) return;
            
            int collidersSize = Physics.OverlapSphereNonAlloc(transform.position, fishingDistance, _fishingColliders, fishingLayerMask);
            for (int i = 0; i < collidersSize; i++)
            {
                IFishingSpot fishingSpot = _fishingColliders[i].GetComponent<IFishingSpot>();
                if (fishingSpot != null)
                {
                    _currentFishingSpot = fishingSpot;
                    _currentFishingSpot.OnFishingRequestProcessed += OnFishingRequestProcessed;
                    _currentFishingSpot.TryFishing(this);
                }
            }
        }

        private void OnFishingRequestProcessed(bool okToFish)
        {
            _currentFishingSpot.OnFishingRequestProcessed -= OnFishingRequestProcessed;
            if(okToFish) OnFishingActionStarted?.Invoke(_currentFishingSpot);
            else OnFishingActionFailed?.Invoke();
        }

        public void CancelFishing()
        {
            if (_currentFishingSpot == null) return;
            
            _currentFishingSpot.OnCanceledFishing();
            _currentFishingSpot = null;
            OnFishingActionCanceled?.Invoke();
        }
    }
}