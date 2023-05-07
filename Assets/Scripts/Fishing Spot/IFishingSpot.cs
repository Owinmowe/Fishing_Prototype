using System;
using FishingPrototype.Gameplay.FishingSpot.Data;
using UnityEngine;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public interface IFishingSpot
    {
        Action<FishingSpotData> OnFishingSpotSet { get; set; } 
        Action<bool> OnFishingRequestProcessed { get; set; }
        Action<int> OnFishAmountChanged { get; set; }
        public Action<FishingSpotData> OnFishingSpotEmpty { get; set; }
        GameObject BaseGameObject { get; }
        void SetFishingSpot(FishingSpotData fishingSpotData);
        FishingSpotData GetFishingSpotData();
        void TryFishing(GameObject fishingGameObject);
        void OnCompletedFishing();
        void OnCanceledFishing();
    }
}
