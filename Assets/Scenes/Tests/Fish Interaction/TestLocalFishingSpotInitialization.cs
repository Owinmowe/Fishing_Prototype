using System;
using FishingPrototype.Gameplay.FishingSpot;
using UnityEngine;

namespace FishingPrototype.Test
{
    public class TestLocalFishingSpotInitialization : MonoBehaviour
    {
        
        [SerializeField] private FishingSpotInitializationData[] initializationData;

        private void Start()
        {
            foreach (var data in initializationData)
            {
                data.fishingSpot.SetFishingSpot(data.fishingSpotType, data.amount);
            }
        }

        [Serializable]
        public struct FishingSpotInitializationData
        {
            public LocalFishingSpot fishingSpot;
            public FishingSpotType fishingSpotType;
            public int amount;
        }
    }
}
