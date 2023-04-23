using System;
using FishingPrototype.Gameplay.FishingSpot;
using Mirror;
using UnityEngine;

namespace FishingPrototype.Test
{
    public class TestNetworkFishingSpotInitialization : NetworkBehaviour
    {
        
        [SerializeField] private FishingSpotInitializationData[] initializationData;

        public override void OnStartServer()
        {
            base.OnStartServer();
            foreach (var data in initializationData)
            {
                NetworkFishingSpot fishingSpot = Instantiate(data.fishingSpot);
                fishingSpot.onSpawned += delegate
                {
                    fishingSpot.SetFishingSpot(data.fishingSpotType, data.amount);
                };
                NetworkServer.Spawn(fishingSpot.gameObject);
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
