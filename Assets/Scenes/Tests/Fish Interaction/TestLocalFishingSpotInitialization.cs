using System;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.FishingSpot.Data;
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
                data.fishingSpot.SetFishingSpot(data.fishingSpotData);
            }
        }

        [Serializable]
        public struct FishingSpotInitializationData
        {
            public LocalFishingSpot fishingSpot;
            public FishingSpotData fishingSpotData;
        }
    }
}
