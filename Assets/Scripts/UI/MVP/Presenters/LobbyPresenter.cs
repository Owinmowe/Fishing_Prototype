using System.Collections.Generic;
using FishingPrototype.Boat.Data;
using FishingPrototype.Gameplay.GameMode;
using FishingPrototype.Gameplay.GameMode.Data;
using FishingPrototype.Gameplay.Logic;
using FishingPrototype.MVP.Data;
using FishingPrototype.MVP.View;
using FishingPrototype.Network.Data;
using Mirror;

namespace FishingPrototype.MVP.Presenter
{
    public class LobbyPresenter: Presenter<GameView>
    {

        private readonly List<PlayerData> _playersData = new List<PlayerData>();
        private IGameLogic _gameLogic;
        private LobbyData _lobbyData;
        
        public LobbyPresenter(GameView view, LobbyData scriptableData) : base(view)
        {
            _lobbyData = scriptableData;
        }

        protected override void AddViewListeners()
        {
            view.OnGameLogicSetEvent += OnGameLogicSet;
            view.OnGameLogicRemovedEvent += OnGameLogicRemoved;
            view.OnPlayerConnectEvent += OnPlayerConnected;
            view.OnPlayerDisconnectEvent += OnPlayerDisconnected;
            view.OnStartGamePressed += OnStartGamePressed;
            view.OnGameStarted += OnGameStarted;
        }

        protected override void RemoveViewListeners()
        {
            view.OnGameLogicSetEvent -= OnGameLogicSet;
            view.OnGameLogicRemovedEvent -= OnGameLogicRemoved;
            view.OnPlayerConnectEvent -= OnPlayerConnected;
            view.OnPlayerDisconnectEvent -= OnPlayerDisconnected;
            view.OnStartGamePressed -= OnStartGamePressed;
            view.OnGameStarted += OnGameStarted;
        }

        private void OnGameLogicSet(IGameLogic gameLogic) => _gameLogic = gameLogic;
        private void OnGameLogicRemoved() => _gameLogic = null;
        
        private void OnPlayerConnected(PlayerReferences playerReferences, NetworkConnection connection)
        {
            _playersData.Add(playerReferences.playerData);
            view.SetConnectedPlayersPanel(_playersData.ToArray());
        }

        private void OnPlayerDisconnected(PlayerReferences playerReferences)
        {
            _playersData.Remove(playerReferences.playerData);
            view.SetConnectedPlayersPanel(_playersData.ToArray());
        }

        private void OnStartGamePressed()
        {
            _gameLogic.StartGame(_lobbyData.gamesModeList.gameModes[0]); // TODO Replace with real game mode selection
        }

        private void OnGameStarted()
        {
            view.CloseLobbyScreen();
            view.OpenGameplayScreen();
        }
    }
}
