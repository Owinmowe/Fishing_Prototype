using System;
using UnityEngine;
using FishingPrototype.MVP.Data;
using FishingPrototype.MVP.Presenter;
using FishingPrototype.MVP.Control;


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
        [SerializeField] private GameplayScreenControl gameplayScreenScreenControl;

        private GameplayPresenter _gameplayPresenter;
        private StartScreenPresenter _startScreenPresenter;
        private LobbyPresenter _lobbyPresenter;
        private FindLobbiesPresenter _findLobbiesPresenter;

        #region Events

        public Action OnExitButtonEvent;
        public Action OnHostLobbyButtonEvent;
        public Action OnJoinLobbyButtonEvent;

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
        }

        private void RemoveControlEvents()
        {
            startScreenControl.OnExitButtonPressed -= OnExitButtonEvent;
            startScreenControl.OnHostLobbyButtonPressed -= OnHostLobbyButtonEvent;
            startScreenControl.OnJoinLobbyButtonPressed -= OnJoinLobbyButtonEvent;
        }

        public void OpenStartScreen() => startScreenControl.OpenScreen();
        public void CloseStartScreen() => startScreenControl.CloseScreen();
        
        public void OpenLobbyScreen() => lobbyScreenControl.OpenScreen();
        public void CloseLobbyScreen() => lobbyScreenControl.CloseScreen();
        public void OpenFindLobbiesScreen() => findLobbiesScreenControl.OpenScreen();
        public void CloseFindLobbiesScreen() => findLobbiesScreenControl.CloseScreen();
        
        public void JoinLobby(ulong lobbyId) => lobbyScreenControl.JoinLobby(lobbyId: lobbyId);
        public void HostLobby() => lobbyScreenControl.HostLobby();

    }
}

