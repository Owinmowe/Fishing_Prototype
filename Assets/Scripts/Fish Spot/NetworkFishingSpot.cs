using System;
using FishingPrototype.Gameplay.Boat;
using Mirror;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public class NetworkFishingSpot : NetworkBehaviour, IFishingSpot
    {
        public Action onSpawned;
        public Action<bool> OnFishingRequestProcessed { get; set; }
        public Action<int> OnFishAmountChanged { get; set; }

        [SyncVar] private FishingSpotType _fishingSpotType;
        [SyncVar(hook = nameof(UpdateFishAmount))] private int _amount;
        private bool _locked;

        public void Start()
        {
            onSpawned?.Invoke();
        }

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
            CmdCompletedFishing();
        }

        [Command(requiresAuthority = false)]
        private void CmdCompletedFishing()
        {
            _amount--;
            if(_amount <= 0)
                NetworkServer.UnSpawn(gameObject);
        }
        
        void UpdateFishAmount(int oldAmount, int newAmount)
        {
            OnFishAmountChanged?.Invoke(newAmount);
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