namespace FishingPrototype.Gameplay.Boat
{
    public interface IBoat
    {
        void ReceiveAcceleration(float accelerationRate);
        void ReceiveRotation(float rotationRate);
    }
}
