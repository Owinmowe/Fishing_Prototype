using System;
using UnityEngine;
using Cinemachine;
using FishingPrototype.Gameplay.Boat;
using FishingPrototype.Gameplay.Input;
using UnityEngine.InputSystem;

namespace FishingPrototype.Gameplay.Camera
{
    public class CameraControl : MonoBehaviour
    {

        [Header("Gameplay Configurations")]
        [SerializeField] private float cameraRotationSpeed = 1f;
        
        private CinemachineVirtualCamera _virtualCamera;
        
        private Transform _boatTransform;
        private Transform _followTargetTransform;
        private bool _rotating;
        private float _rotatingAmount;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            
            IBoat.onLocalBoatSet += delegate(IBoat boat)
            {
                _boatTransform = boat.BaseGameObject.transform;
                _followTargetTransform = new GameObject
                {
                    transform =
                    {
                        name = "Follow Target",
                        position = _boatTransform.position,
                        rotation = _boatTransform.rotation
                    }
                }.transform;

                _virtualCamera.m_Follow = _followTargetTransform;
                _virtualCamera.m_LookAt = _followTargetTransform;
            };

            CustomInput.Input.CameraControl.Rotate.performed += StartCameraRotate;
            CustomInput.Input.CameraControl.Rotate.canceled += StopCameraRotate;
        }
        
        private void StartCameraRotate(InputAction.CallbackContext context)
        {
            _rotatingAmount = context.ReadValue<float>();
            _rotating = true;
        }

        private void StopCameraRotate(InputAction.CallbackContext context) => _rotating = false;

        //This is in FixedUpdate because the boat is moving with Physics, so follow target position must update with boat update
        private void FixedUpdate()
        {
            if (_followTargetTransform != null)
            {
                _followTargetTransform.position = _boatTransform.position;

                if (_rotating)
                {
                    _followTargetTransform.Rotate(Vector3.up, _rotatingAmount * cameraRotationSpeed * Time.fixedDeltaTime);
                }
            }
        }

        private void OnDestroy()
        {
            CustomInput.Input.CameraControl.Rotate.performed -= StartCameraRotate;
            CustomInput.Input.CameraControl.Rotate.canceled -= StopCameraRotate;
        }
    }
}
