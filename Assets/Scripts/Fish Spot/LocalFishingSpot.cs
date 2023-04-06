using System;
using FishingPrototype.Gameplay.Boat;
using UnityEngine;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public class LocalFishingSpot : MonoBehaviour, IFishingSpot
    {
        public Action<bool> OnFishingRequestProcessed { get; set; }
        public Action<int> OnFishAmountChanged { get; set; }

        private FishingSpotType _fishingSpotType;
        private int _amount;
        private bool _locked;

        public void SetFishingSpot(FishingSpotType type, int amount)
        {
            _fishingSpotType = type;
            _amount = amount;
        }

        public Tuple<FishingSpotType, int> GetFishingSpotData()
        {
            return new Tuple<FishingSpotType, int>(_fishingSpotType, _amount);
        }
        
        public void TryFishing(IBoat boat)
        {
            OnFishingRequestProcessed?.Invoke(!_locked);
            _locked = true;
        }

        public void OnCompletedFishing()
        {
            _amount--;
            OnFishAmountChanged?.Invoke(_amount);
            if(_amount <= 0)
                Destroy(gameObject);
        }

        public void OnCanceledFishing()
        {
            _locked = false;
        }
    }
}
