using System.Collections.Generic;
using FishingPrototype.Boat.Data;
using FishingPrototype.MVP.Data;
using FishingPrototype.MVP.View;
using FishingPrototype.Network;
using Mirror;

namespace FishingPrototype.MVP.Presenter
{
    public class LobbyPresenter: Presenter<GameView>
    {

        private List<PlayerData> _playersData = new List<PlayerData>();

        public LobbyPresenter(GameView view, LobbyData scriptableData) : base(view)
        {
            
        }

        protected override void AddViewListeners()
        {
            view.OnPlayerConnectEvent += OnPlayerConnected;
            view.OnPlayerDisconnectEvent += OnPlayerDisconnected;
        }

        protected override void RemoveViewListeners()
        {
            view.OnPlayerConnectEvent -= OnPlayerConnected;
            view.OnPlayerDisconnectEvent -= OnPlayerDisconnected;
        }

        private void OnPlayerConnected(CustomNetworkManager.PlayerReferences playerReferences, NetworkConnection connection)
        {
            _playersData.Add(playerReferences.playerData);
            view.SetConnectedPlayersPanel(_playersData.ToArray());
        }

        private void OnPlayerDisconnected(CustomNetworkManager.PlayerReferences playerReferences)
        {
            _playersData.Remove(playerReferences.playerData);
            view.SetConnectedPlayersPanel(_playersData.ToArray());
        }

    }
}
