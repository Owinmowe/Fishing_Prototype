using System;
using FishingPrototype.Player;
using FishingPrototype.Player.Data;
using Mirror;

namespace FishingPrototype.Gameplay.Boat
{
    public class NetworkPlayerDataControl : NetworkBehaviour, IPlayerDataControl
    {
        public Action<PlayerData> OnNewPlayerDataSet { get; set; }

        public void SetPlayerData(PlayerData data)
        {
            OnNewPlayerDataSet?.Invoke(data);
            RpcSetPlayerData(data);
        }

        [ClientRpc]
        private void RpcSetPlayerData(PlayerData data)
        {
            OnNewPlayerDataSet?.Invoke(data);
        }
    }
}
