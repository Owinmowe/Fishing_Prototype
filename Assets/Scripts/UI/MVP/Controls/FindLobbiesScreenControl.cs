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

        public void OpenScreen()
        {
            gameObject.SetActive(true);
            CustomNetworkManager.Instance.OnLobbiesGet += OnLobbiesGetEvent;
            CustomNetworkManager.Instance.OnLobbyJoined += OnJoinedLobby;
        }
        
        public void CloseScreen()
        {
            CustomNetworkManager.Instance.OnLobbiesGet -= OnLobbiesGetEvent;
            CustomNetworkManager.Instance.OnLobbyJoined -= OnJoinedLobby;
            gameObject.SetActive(false);
        }


        public void RefreshLobbies() => CustomNetworkManager.Instance.RequestLobbiesList();
        
        public void JoinLobby(ulong lobbyId) => CustomNetworkManager.Instance.RequestJoinLobby(lobbyId: lobbyId);
    }
}