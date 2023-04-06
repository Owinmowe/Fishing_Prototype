using System;
using FishingPrototype.Gameplay.Boat;
using Mirror;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public class NetworkFishingSpot : NetworkBehaviour, IFishingSpot
    {
        public Action<bool> OnFishingRequestProcessed { get; set; }
        public Action<int> UpdateFishAmount { get; set; }

        private FishingSpotType _fishingSpotType;
        private int _amount;
        private bool _locked;

        public void SetFishingSpot(FishingSpotType type, int amount)
        {
            _fishingSpotType = type;
            _amount = amount;
            RpcSetFishingSpot(type, amount);
        }

        [ClientRpc]
        private void RpcSetFishingSpot(FishingSpotType type, int amount)
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
            uint boatId = boat.BaseGameObject.GetComponent<NetworkIdentity>().netId;
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
            _amount--;
            CmdCompletedFishing();
        }

        [Command(requiresAuthority = false)]
        private void CmdCompletedFishing()
        {
            _amount--;
            RpcUpdateFishAmount(_amount);
            if(_amount <= 0)
                NetworkServer.UnSpawn(gameObject);
        }

        [ClientRpc]
        private void RpcUpdateFishAmount(int amount)
        {
            UpdateFishAmount?.Invoke(amount);
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