using System.Collections.Generic;
using FishingPrototype.Network;
using FishingPrototype.Network.Data;
using UnityEngine;

namespace FishingPrototype.MVP.Control
{
    public class FindLobbiesScreenControl : MonoBehaviour, IScreenControl
    {

        public System.Action<List<SteamLobbyData>> OnLobbiesGetEvent;
        public System.Action OnJoinedLobby;
        
        [SerializeField] private CustomNetworkManager customNetworkManager;
        
        public void OpenScreen()
        {
            gameObject.SetActive(true);
            customNetworkManager.OnLobbiesGet += OnLobbiesGetEvent;
            customNetworkManager.OnLobbyJoined += OnJoinedLobby;
        }
        
        public void CloseScreen()
        {
            customNetworkManager.OnLobbiesGet -= OnLobbiesGetEvent;
            customNetworkManager.OnLobbyJoined -= OnJoinedLobby;
            gameObject.SetActive(false);
        }


        public void RefreshLobbies() => customNetworkManager.RequestLobbiesList();
        
        public void JoinLobby(ulong lobbyId) => customNetworkManager.RequestJoinLobby(lobbyId: lobbyId);
    }
}