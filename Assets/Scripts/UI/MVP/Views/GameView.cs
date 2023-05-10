using System;
using System.Collections.Generic;
using FishingPrototype.Gameplay.Boat;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.Input;
using FishingPrototype.Gameplay.Logic;
using FishingPrototype.Gameplay.Minigames;
using UnityEngine;
using FishingPrototype.MVP.Data;
using FishingPrototype.MVP.Presenter;
using FishingPrototype.MVP.Control;
using FishingPrototype.Network;
using FishingPrototype.Network.Data;
using FishingPrototype.Player.Data;
using Mirror;
using UnityEngine.InputSystem;


namespace FishingPrototype.MVP.View
{
    public class GameView : MonoBehaviour
    {
        
        [Header("Data")] 
        [SerializeField] private StartScreenData startScreenData;
        [SerializeField] private FindLobbiesData findLobbiesData;
        [SerializeField] private LobbyData lobbyData;
        [SerializeField] private GameplayData gameplayData;

        [Header("Controls")] 
        [SerializeField] private StartScreenControl startScreenControl;
        [SerializeField] private FindLobbiesScreenControl findLobbiesScreenControl;
        [SerializeField] private LobbyScreenControl lobbyScreenControl;
        [SerializeField] private GameplayScreenControl gameplayScreenControl;

        [Header("Scene References")]
        
        private GameplayPresenter _gameplayPresenter;
        private StartScreenPresenter _startScreenPresenter;
        private LobbyPresenter _lobbyPresenter;
        private FindLobbiesPresenter _findLobbiesPresenter;
        
        private IGameLogic _gameLogic;

        #region START_SCREEN_EVENTS

        public event Action OnExitButtonEvent;
        public event Action OnHostLobbyButtonEvent;
        public event Action OnJoinLobbyButtonEvent;

        #endregion
        
        #region FIND_LOBBIES_SCREEN_EVENTS

        public event Action<List<SteamLobbyData>> OnFindLobbiesEvent;
        public event Action OnJoinLobbyEvent;
        
        #endregion
        
        #region LOBBY_SCREEN_EVENTS
        
        public event Action<PlayerReferences, NetworkConnection> OnPlayerConnectEvent;
        public event Action<PlayerReferences, int> OnPlayerDisconnectEvent;
        public event Action OnStartGamePressed;
        public event Action OnGameStarted;
        
        #endregion
        
        #region GAMEPLAY_SCREEN_EVENTS

        public event Action<IGameLogic> OnGameLogicSetEvent; 
        public event Action OnGameLogicRemovedEvent; 
        public event Action<IBoat> OnLocalBoatSetEvent;
        public event Action<IBoat> OnLocalBoatRemoveEvent;
        public event Action<InputAction.CallbackContext> OnPerformedCustomInput1Event;
        public event Action<InputAction.CallbackContext> OnPerformedCustomInput2Event;
        public event Action<InputAction.CallbackContext> OnCanceledCustomInput1Event;
        public event Action<InputAction.CallbackContext> OnCanceledCustomInput2Event;
        public event Action<IFishingSpot> FishingActionStartedEvent;
        public event Action FishingActionCompletedEvent;
        public event Action FishingActionCanceledEvent;
        public event Action FishingActionFailedEvent;
        
        #endregion
        
        private void Awake()
        {
            _gameplayPresenter = new GameplayPresenter(this, gameplayData);
            _startScreenPresenter = new StartScreenPresenter(this, startScreenData);
            _lobbyPresenter = new LobbyPresenter(this, lobbyData);
            _findLobbiesPresenter = new FindLobbiesPresenter(this, findLobbiesData);

            IGameLogic.OnGameLogicSet += OnGameLogicSet;
        }

        private void Start()
        {
            AddEvents();
        }
        
        private void OnDestroy()
        {
            RemoveEvents();
        }

        private void AddEvents()
        {
            startScreenControl.OnExitButtonPressed += OnExitButtonEvent;
            startScreenControl.OnHostLobbyButtonPressed += OnHostLobbyButtonEvent;
            startScreenControl.OnJoinLobbyButtonPressed += OnJoinLobbyButtonEvent;

            findLobbiesScreenControl.OnLobbiesGetEvent += OnFindLobbiesEvent;
            findLobbiesScreenControl.OnJoinedLobby += OnJoinLobbyEvent;
            
            CustomInput.Input.MiniGamesControl.MiniGameInput1.performed += OnPerformedCustomInput1Event;
            CustomInput.Input.MiniGamesControl.MiniGameInput2.performed += OnPerformedCustomInput2Event;
            CustomInput.Input.MiniGamesControl.MiniGameInput1.canceled += OnCanceledCustomInput1Event;
            CustomInput.Input.MiniGamesControl.MiniGameInput2.canceled += OnCanceledCustomInput2Event;

            CustomNetworkManager.Instance.OnPlayerIdentify += OnPlayerConnectEvent;
            CustomNetworkManager.Instance.OnPlayerDisconnect += OnPlayerDisconnectEvent;

            gameplayScreenControl.OnFishingCompleted += FishingActionCompletedEvent;

            lobbyScreenControl.OnStartButtonPressed += OnStartGamePressed;
            
            IBoat.OnLocalBoatSet += OnLocalBoatSetEvent;
            IBoat.OnLocalBoatRemoved += OnLocalBoatRemoveEvent;
        }

        private void RemoveEvents()
        {
            IGameLogic.OnGameLogicSet -= OnGameLogicSet;
            RemoveGameLogic();
            
            startScreenControl.OnExitButtonPressed -= OnExitButtonEvent;
            startScreenControl.OnHostLobbyButtonPressed -= OnHostLobbyButtonEvent;
            startScreenControl.OnJoinLobbyButtonPressed -= OnJoinLobbyButtonEvent;
            
            findLobbiesScreenControl.OnLobbiesGetEvent -= OnFindLobbiesEvent;
            findLobbiesScreenControl.OnJoinedLobby -= OnJoinLobbyEvent;
            
            CustomInput.Input.MiniGamesControl.MiniGameInput1.performed -= OnPerformedCustomInput1Event; 
            CustomInput.Input.MiniGamesControl.MiniGameInput2.performed -= OnPerformedCustomInput2Event; 
            CustomInput.Input.MiniGamesControl.MiniGameInput1.canceled -= OnCanceledCustomInput1Event;
            CustomInput.Input.MiniGamesControl.MiniGameInput2.canceled -= OnCanceledCustomInput2Event;
            
            CustomNetworkManager.Instance.OnPlayerIdentify -= OnPlayerConnectEvent;
            CustomNetworkManager.Instance.OnPlayerDisconnect -= OnPlayerDisconnectEvent;
            
            gameplayScreenControl.OnFishingCompleted -= FishingActionCompletedEvent;

            lobbyScreenControl.OnStartButtonPressed -= OnStartGamePressed;
            
            IBoat.OnLocalBoatSet -= OnLocalBoatSetEvent;
            IBoat.OnLocalBoatRemoved -= OnLocalBoatRemoveEvent;
        }

        private void OnGameLogicSet(IGameLogic gameLogic)
        {
            _gameLogic = gameLogic;
            _gameLogic.OnGameStarted += OnGameStarted;
            OnGameLogicSetEvent?.Invoke(gameLogic);
        }

        private void RemoveGameLogic()
        {
            if (_gameLogic == null) return;
            
            OnGameLogicRemovedEvent?.Invoke();
            _gameLogic.OnGameStarted -= OnGameStarted;
            _gameLogic = null;
        }
        
        #region START_SCREEN_CONTROL
        
        public void OpenStartScreen() => startScreenControl.OpenScreen();
        public void CloseStartScreen() => startScreenControl.CloseScreen();

        #endregion
        
        #region LOBBY_SCREEN_CONTROL

        public void OpenLobbyScreen() => lobbyScreenControl.OpenScreen();
        public void CloseLobbyScreen() => lobbyScreenControl.CloseScreen();
        public void SetConnectedPlayersPanel(PlayerData[] connectedPlayersData)
            => lobbyScreenControl.SetConnectedPlayersPanel(connectedPlayersData);

        #endregion
        
        #region FIND_LOBBIES_SCREEN_CONTROL
        
        public void OpenFindLobbiesScreen() => findLobbiesScreenControl.OpenScreen();
        public void CloseFindLobbiesScreen() => findLobbiesScreenControl.CloseScreen();
        public void RefreshLobbiesList() => findLobbiesScreenControl.RefreshLobbies();
        public void JoinLobby(ulong lobbyId) => findLobbiesScreenControl.JoinLobby(lobbyId: lobbyId);
        
        #endregion

        #region GAMEPLAY_SCREEN_CONTROL

        public void OpenGameplayScreen() => gameplayScreenControl.OpenScreen();
        public void CloseGameplayScreen() => gameplayScreenControl.CloseScreen();
        public void InjectMiniGames(MiniGameBase[] miniGames) => gameplayScreenControl.InjectMiniGames(miniGames);
        public void PerformCustomInput1() => gameplayScreenControl.PerformCustomInput1();
        public void PerformCustomInput2() => gameplayScreenControl.PerformCustomInput2();
        public void CancelCustomInput1() => gameplayScreenControl.CancelCustomInput1();
        public void CancelCustomInput2() => gameplayScreenControl.CancelCustomInput2();
        public void RegisterLocalBoatEvent(IBoat boat)
        {
            boat.OnFishingActionStarted += FishingActionStartedEvent;
            boat.OnFishingActionCanceled += FishingActionCanceledEvent;
            boat.OnFishingActionFailed += FishingActionFailedEvent;
        }
        public void UnRegisterLocalBoatEvent(IBoat boat)
        {
            boat.OnFishingActionStarted -= FishingActionStartedEvent;
            boat.OnFishingActionCanceled -= FishingActionCanceledEvent;
            boat.OnFishingActionFailed -= FishingActionFailedEvent;
        }

        public void FishingActionStarted(IFishingSpot fishingSpot) => gameplayScreenControl.FishingStarted(fishingSpot);
        public void FishingActionCanceled() => gameplayScreenControl.FishingCanceled();
        public void FishingActionFailed() => gameplayScreenControl.FishingFailed();

        #endregion

    }
}

