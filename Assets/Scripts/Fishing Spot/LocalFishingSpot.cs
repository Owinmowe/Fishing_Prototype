using System;
using FishingPrototype.Gameplay.FishingSpot.Data;
using UnityEngine;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public class LocalFishingSpot : MonoBehaviour, IFishingSpot
    {
        public Action<FishingSpotData> OnFishingSpotSet { get; set; }
        public Action<bool> OnFishingRequestProcessed { get; set; }
        public Action<int> OnFishAmountChanged { get; set; }
        public Action<FishingSpotData> OnFishingSpotEmpty { get; set; }
        public GameObject BaseGameObject => gameObject;

        private FishingSpotData _fishingSpotData;
        private bool _locked;


        public void SetFishingSpot(FishingSpotData fishingSpotData)
        {
            _fishingSpotData = fishingSpotData;
            gameObject.name = "Local " + Enum.GetName(typeof(FishingSpotType), _fishingSpotData.type) + " Fishing Spot";
            OnFishingSpotSet?.Invoke(_fishingSpotData);
        }

        public FishingSpotData GetFishingSpotData()
        {
            return _fishingSpotData;
        }
        
        public void TryFishing(GameObject fishingGameObject)
        {
            OnFishingRequestProcessed?.Invoke(!_locked);
            _locked = true;
        }

        public void OnCompletedFishing()
        {
            _fishingSpotData.amount--;
            OnFishAmountChanged?.Invoke(_fishingSpotData.amount);
            if (_fishingSpotData.amount <= 0)
            {
                OnFishingSpotEmpty?.Invoke(_fishingSpotData);
                Destroy(gameObject);
            }
        }

        public void OnCanceledFishing()
        {
            _locked = false;
        }
    }
}
