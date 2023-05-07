using System;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.FishingSpot.Data;
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
                    fishingSpot.SetFishingSpot(data.fishingSpotData);
                };
                NetworkServer.Spawn(fishingSpot.gameObject);
            }
        }

        [Serializable]
        public struct FishingSpotInitializationData
        {
            public NetworkFishingSpot fishingSpot;
            public FishingSpotData fishingSpotData;
        }
    }
}
