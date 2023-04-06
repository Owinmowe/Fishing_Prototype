using System;
using FishingPrototype.Gameplay.FishingSpot;
using Mirror;
using UnityEngine;

namespace FishingPrototype.Test
{
    public class TestNetworkFishingSpotInitialization : NetworkBehaviour
    {
        
        [SerializeField] private FishingSpotInitializationData[] initializationData;

        private void Start()
        {
            if (!isServer) return;
            
            foreach (var data in initializationData)
            {
                data.fishingSpot.SetFishingSpot(data.fishingSpotType, data.amount);
            }
        }

        [Serializable]
        public struct FishingSpotInitializationData
        {
            public NetworkFishingSpot fishingSpot;
            public FishingSpotType fishingSpotType;
            public int amount;
        }
    }
}
