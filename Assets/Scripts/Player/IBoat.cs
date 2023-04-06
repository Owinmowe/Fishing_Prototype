using FishingPrototype.Gameplay.FishingSpot;

namespace FishingPrototype.Gameplay.Boat
{
    public interface IBoat
    {
        public static System.Action<IBoat> onLocalBoatSet; 
        public event System.Action<IFishingSpot> OnFishingActionStarted;
        public event System.Action OnFishingActionFailed;
        public event System.Action OnFishingActionCanceled;
        void ReceiveAcceleration(float accelerationRate);
        void ReceiveRotation(float rotationRate);
        void TryFishing();
        void CancelFishing();
    }
}
