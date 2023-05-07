using System;
using UnityEngine;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public interface IFishingSpot
    {
        Action<Tuple<FishingSpotType, int>> OnFishingSpotSet { get; set; } 
        Action<bool> OnFishingRequestProcessed { get; set; }
        Action<int> OnFishAmountChanged { get; set; }
        public Action<FishingSpotType> OnFishingSpotEmpty { get; set; }
        GameObject BaseGameObject { get; }
        void SetFishingSpot(FishingSpotType type, int amount);
        Tuple<FishingSpotType, int> GetFishingSpotData();
        void TryFishing(GameObject fishingGameObject);
        void OnCompletedFishing();
        void OnCanceledFishing();
    }
}
