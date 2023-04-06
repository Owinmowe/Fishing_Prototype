using FishingPrototype.Gameplay.Boat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingPrototype.Gameplay.Input
{
    public class LocalInput : MonoBehaviour
    {

        private IBoat _boat;

        private bool accelerateInputReceived = false;
        private bool rotateInputReceived = false;
        
        private void Awake()
        {
            _boat = GetComponent<IBoat>();

            CustomInput.Input.BoatControl.Accelerate.performed += OnAccelerateInputPerformed;
            CustomInput.Input.BoatControl.Accelerate.canceled += OnAccelerateInputCanceled;
            CustomInput.Input.BoatControl.Rotate.performed += OnRotateInputPerformed;
            CustomInput.Input.BoatControl.Rotate.canceled += OnRotateInputCanceled;
            
            CustomInput.Input.BoatControl.TryFishing.performed += OnFishingInputPerformed;
            CustomInput.Input.BoatControl.CancelFishing.performed += OnFishingCancelInputPerformed;
        }

        private void Update()
        {
            if(accelerateInputReceived)
                _boat.ReceiveAcceleration(CustomInput.Input.BoatControl.Accelerate.ReadValue<float>());
            
            if(rotateInputReceived)
                _boat.ReceiveRotation(CustomInput.Input.BoatControl.Rotate.ReadValue<float>());
        }

        private void OnFishingInputPerformed(InputAction.CallbackContext context) => _boat.TryFishing();
        private void OnFishingCancelInputPerformed(InputAction.CallbackContext context) => _boat.CancelFishing();
        
        private void OnAccelerateInputPerformed(InputAction.CallbackContext context) => accelerateInputReceived = true;
        private void OnAccelerateInputCanceled(InputAction.CallbackContext context) => accelerateInputReceived = false;
        
        private void OnRotateInputPerformed(InputAction.CallbackContext context) => rotateInputReceived = true;
        private void OnRotateInputCanceled(InputAction.CallbackContext context) => rotateInputReceived = false;
        
        private void OnDestroy()
        {
            CustomInput.Input.BoatControl.Accelerate.performed -= OnAccelerateInputPerformed;
            CustomInput.Input.BoatControl.Accelerate.canceled += OnAccelerateInputCanceled;
            CustomInput.Input.BoatControl.Rotate.performed -= OnRotateInputPerformed;
            CustomInput.Input.BoatControl.Rotate.canceled -= OnRotateInputCanceled;
            
            CustomInput.Input.BoatControl.TryFishing.performed -= OnFishingInputPerformed;
            CustomInput.Input.BoatControl.CancelFishing.performed -= OnFishingCancelInputPerformed;
        }
    }
}
