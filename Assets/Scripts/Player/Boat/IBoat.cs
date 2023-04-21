using FishingPrototype.Gameplay.FishingSpot;
using UnityEngine;

namespace FishingPrototype.Gameplay.Boat
{
    public interface IBoat
    {
        public static System.Action<IBoat> OnLocalBoatSet; 
        public static System.Action<IBoat> OnLocalBoatRemoved; 
        public event System.Action<IFishingSpot> OnFishingActionStarted;
        public event System.Action OnFishingActionFailed;
        public event System.Action OnFishingActionCanceled;
        public GameObject BaseGameObject { get; }
        public Transform FollowTarget { get; }
        void ReceiveAcceleration(float accelerationRate);
        void ReceiveRotation(float rotationRate);
        void TryFishing();
        void CancelFishing();
        void CompleteFishing();
    }
}
