using System;
using System.Collections;
using UnityEngine;
using Steamworks;

namespace FishingPrototype.Network.Steam
{
    public class SteamEventDispatcher : MonoBehaviour //TODO Make a better dispatcher that ensures events order with Queues, dynamic delegates and reflection or something like that, i don't know, i'm an idiot with a computer
    {

        public static event Action<LobbyCreated_t> LobbyCreatedEvent;
        public static event Action<GameLobbyJoinRequested_t> JoinRequestEvent;
        public static event Action<LobbyEnter_t> LobbyEnteredEvent;
        public static event Action<LobbyMatchList_t> LobbyMatchListReturnedEvent;
        private void OnEnable()
        {
            if (!SteamManager.Initialized)
                return;

            Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            Callback<LobbyMatchList_t>.Create(OnLobbyMatchListReturned);
        }

        private void OnLobbyCreated(LobbyCreated_t callback) => StartCoroutine(OnLobbyCreatedCoroutine(callback));
        private IEnumerator OnLobbyCreatedCoroutine(LobbyCreated_t callback)
        {
            yield return null;
            LobbyCreatedEvent?.Invoke(callback);
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback) => StartCoroutine(OnJoinRequestCoroutine(callback));
        private IEnumerator OnJoinRequestCoroutine(GameLobbyJoinRequested_t callback)
        {
            yield return null;
            JoinRequestEvent?.Invoke(callback);
        }
        
        private void OnLobbyEntered(LobbyEnter_t callback) => StartCoroutine(OnLobbyEnteredCoroutine(callback));
        private IEnumerator OnLobbyEnteredCoroutine(LobbyEnter_t callback)
        {
            yield return null;
            LobbyEnteredEvent?.Invoke(callback);
        }
        
        private void OnLobbyMatchListReturned(LobbyMatchList_t callback) => StartCoroutine(OnLobbyMatchListReturnedCoroutine(callback));
        private IEnumerator OnLobbyMatchListReturnedCoroutine(LobbyMatchList_t callback)
        {
            yield return null;
            LobbyMatchListReturnedEvent?.Invoke(callback);
        }

    }
}