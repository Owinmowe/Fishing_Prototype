using UnityEngine;
using Cinemachine;
using FishingPrototype.Gameplay.Boat;

namespace FishingPrototype.Gameplay.Camera
{
    public class CameraControl : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            
            IBoat.onLocalBoatSet += delegate(IBoat boat)
            {
                _virtualCamera.m_Follow = boat.BaseGameObject.transform;
                _virtualCamera.m_LookAt = boat.BaseGameObject.transform;
            };
        }
    }
}
