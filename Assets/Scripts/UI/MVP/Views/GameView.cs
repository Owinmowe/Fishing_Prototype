using System;
using System.Collections.Generic;
using FishingPrototype.Gameplay.Boat;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.Input;
using FishingPrototype.Gameplay.Minigames;
using UnityEngine;
using FishingPrototype.MVP.Data;
using FishingPrototype.MVP.Presenter;
using FishingPrototype.MVP.Control;
using FishingPrototype.Network.Data;
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

        #region EVENTS

        public event Action OnExitButtonEvent;
        public event Action OnHostLobbyButtonEvent;
        public event Action OnJoinLobbyButtonEvent;

        public event Action<List<SteamLobbyData>> OnFindLobbiesEvent;
        public event Action OnJoinLobbyEvent;

        public event Action<IBoat> OnLocalBoatSetEvent;
        public event Action<IBoat> OnLocalBoatRemoveEvent;
        public event Action<InputAction.CallbackContext> OnPerformedCustomInput1Event;
        public event Action<InputAction.CallbackContext> OnPerformedCustomInput2Event;
        public event Action<IFishingSpot> FishingActionStartedEvent;
        public event Action FishingActionCanceledEvent;
        public event Action FishingActionFailedEvent;
        
        #endregion
        
        private void Awake()
        {
            _gameplayPresenter = new GameplayPresenter(this, gameplayData);
            _startScreenPresenter = new StartScreenPresenter(this, startScreenData);
            _lobbyPresenter = new LobbyPresenter(this, lobbyData);
            _findLobbiesPresenter = new FindLobbiesPresenter(this, findLobbiesData);
        }

        private void Start()
        {
            AddControlEvents();
        }
        
        private void OnDestroy()
        {
            RemoveControlEvents();
        }

        private void AddControlEvents()
        {
            startScreenControl.OnExitButtonPressed += OnExitButtonEvent;
            startScreenControl.OnHostLobbyButtonPressed += OnHostLobbyButtonEvent;
            startScreenControl.OnJoinLobbyButtonPressed += OnJoinLobbyButtonEvent;

            findLobbiesScreenControl.OnLobbiesGetEvent += OnFindLobbiesEvent;
            findLobbiesScreenControl.OnJoinedLobby += OnJoinLobbyEvent;
            
            CustomInput.Input.MiniGamesControl.MiniGameInput1.performed += OnPerformedCustomInput1Event;
            CustomInput.Input.MiniGamesControl.MiniGameInput2.performed += OnPerformedCustomInput2Event;
            
            IBoat.OnLocalBoatSet += OnLocalBoatSetEvent;
            IBoat.OnLocalBoatRemoved += OnLocalBoatRemoveEvent;
        }

        private void RemoveControlEvents()
        {
            startScreenControl.OnExitButtonPressed -= OnExitButtonEvent;
            startScreenControl.OnHostLobbyButtonPressed -= OnHostLobbyButtonEvent;
            startScreenControl.OnJoinLobbyButtonPressed -= OnJoinLobbyButtonEvent;
            
            findLobbiesScreenControl.OnLobbiesGetEvent -= OnFindLobbiesEvent;
            findLobbiesScreenControl.OnJoinedLobby -= OnJoinLobbyEvent;
            
            CustomInput.Input.MiniGamesControl.MiniGameInput1.performed -= OnPerformedCustomInput1Event; 
            CustomInput.Input.MiniGamesControl.MiniGameInput2.performed -= OnPerformedCustomInput2Event; 
            
            IBoat.OnLocalBoatSet -= OnLocalBoatSetEvent;
            IBoat.OnLocalBoatRemoved -= OnLocalBoatRemoveEvent;
        }

        #region START_SCREEN_CONTROL
        
        public void OpenStartScreen() => startScreenControl.OpenScreen();
        public void CloseStartScreen() => startScreenControl.CloseScreen();
        public void HostLobby() => startScreenControl.HostLobby();

        #endregion
        
        #region LOBBY_SCREEN_CONTROL

        public void OpenLobbyScreen() => lobbyScreenControl.OpenScreen();
        public void CloseLobbyScreen() => lobbyScreenControl.CloseScreen();

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

