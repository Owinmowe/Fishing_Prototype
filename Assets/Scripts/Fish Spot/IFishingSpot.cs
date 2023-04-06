using System;
using FishingPrototype.Gameplay.Boat;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public interface IFishingSpot
    {
        Action<bool> OnFishingRequestProcessed { get; set; }
        Action<int> OnFishAmountChanged { get; set; }
        void SetFishingSpot(FishingSpotType type, int amount);
        Tuple<FishingSpotType, int> GetFishingSpotData();
        void TryFishing(IBoat boat);
        void OnCompletedFishing();
        void OnCanceledFishing();
    }
}
