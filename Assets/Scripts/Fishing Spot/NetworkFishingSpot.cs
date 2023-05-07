using System;
using FishingPrototype.Gameplay.FishingSpot.Data;
using Mirror;
using UnityEngine;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public class NetworkFishingSpot : NetworkBehaviour, IFishingSpot
    {
        public Action onSpawned;
        public Action<FishingSpotData> OnFishingSpotSet { get; set; }
        public Action<bool> OnFishingRequestProcessed { get; set; }
        public Action<int> OnFishAmountChanged { get; set; }
        public Action<FishingSpotData> OnFishingSpotEmpty { get; set; }
        public GameObject BaseGameObject => gameObject;

        [SyncVar(hook = nameof(UpdateFishAmount))] private FishingSpotData _fishingSpotData;
        private bool _locked;

        public void Start()
        {
            onSpawned?.Invoke();
        }
        
        public void SetFishingSpot(FishingSpotData fishingSpotData)
        {
            _fishingSpotData = fishingSpotData;
            RpcSetFishingSpot();
        }

        [ClientRpc(includeOwner = true)]
        private void RpcSetFishingSpot()
        {
            gameObject.name = "Network " + Enum.GetName(typeof(FishingSpotType), _fishingSpotData.type) + " Fishing Spot";
            OnFishingSpotSet?.Invoke(_fishingSpotData);
        }
        
        public FishingSpotData GetFishingSpotData()
        {
            return _fishingSpotData;
        }
        
        public void TryFishing(GameObject fishingGameObject)
        {
            uint boatId = fishingGameObject.GetComponent<NetworkIdentity>().netId;
            CmdTryFishing(boatId);
        }

        [Command(requiresAuthority = false)]
        private void CmdTryFishing(uint boatId)
        {
            NetworkIdentity boatNetworkIdentity = NetworkServer.spawned[boatId];
            TargetFishingResponse(boatNetworkIdentity.connectionToClient, !_locked);
            
            if (!_locked)
            {
                _locked = true;
            }
        }

        [TargetRpc]
        private void TargetFishingResponse(NetworkConnectionToClient target, bool okToFish)
        {
            OnFishingRequestProcessed?.Invoke(okToFish);
        }

        public void OnCompletedFishing()
        {
            CmdCompletedFishing();
        }

        [Command(requiresAuthority = false)]
        private void CmdCompletedFishing()
        {
            _fishingSpotData.amount--;
            if (_fishingSpotData.amount <= 0)
            {
                OnFishingSpotEmpty?.Invoke(_fishingSpotData);
                NetworkServer.UnSpawn(gameObject);
                Destroy(gameObject);
            }
        }
        
        void UpdateFishAmount(FishingSpotData oldFishingSpotData, FishingSpotData newFishingSpotData)
        {
            if(oldFishingSpotData.amount != newFishingSpotData.amount)
                OnFishAmountChanged?.Invoke(newFishingSpotData.amount);
        }
        
        public void OnCanceledFishing()
        {
            CmdCanceledFishing();
        }

        [Command(requiresAuthority = false)]
        private void CmdCanceledFishing()
        {
            _locked = false;
        }
    }
}