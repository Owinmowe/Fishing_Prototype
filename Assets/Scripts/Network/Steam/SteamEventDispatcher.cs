using System;
using System.Collections.Generic;
using System.Reflection;
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

        private readonly Queue<SteamEvent> _steamEventsQueue = new Queue<SteamEvent>();
        
        private void OnEnable()
        {
            if (!SteamManager.Initialized)
                return;

            Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            Callback<LobbyMatchList_t>.Create(OnLobbyMatchListReturned);
        }

        private void Update()
        {
            while (_steamEventsQueue.Count > 0)
            {
                _steamEventsQueue.Dequeue().CallEvent();
            }
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            MethodInfo methodInfo = typeof(SteamEventDispatcher).GetMethod(nameof(LobbyCreatedMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            SteamEvent steamEvent = new SteamEvent(targetObject: this, actionObject: callback, callMethod: methodInfo);
            _steamEventsQueue.Enqueue(steamEvent);
        }
        private void LobbyCreatedMethod(LobbyCreated_t callback) => LobbyCreatedEvent?.Invoke(callback);

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            MethodInfo methodInfo = typeof(SteamEventDispatcher).GetMethod(nameof(OnJoinRequestMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            SteamEvent steamEvent = new SteamEvent(targetObject: this, actionObject: callback, callMethod: methodInfo);
            _steamEventsQueue.Enqueue(steamEvent);
        }
        private void OnJoinRequestMethod(GameLobbyJoinRequested_t callback) => JoinRequestEvent?.Invoke(callback);
        
        
        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            MethodInfo methodInfo = typeof(SteamEventDispatcher).GetMethod(nameof(OnLobbyEnteredMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            SteamEvent steamEvent = new SteamEvent(targetObject: this, actionObject: callback, callMethod: methodInfo);
            _steamEventsQueue.Enqueue(steamEvent);
        }
        private void OnLobbyEnteredMethod(LobbyEnter_t callback) => LobbyEnteredEvent?.Invoke(callback);

        private void OnLobbyMatchListReturned(LobbyMatchList_t callback)
        {
            MethodInfo methodInfo = typeof(SteamEventDispatcher).GetMethod(nameof(OnLobbyMatchListReturnedMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            SteamEvent steamEvent = new SteamEvent(targetObject: this, actionObject: callback, callMethod: methodInfo);
            _steamEventsQueue.Enqueue(steamEvent);
        }
        private void OnLobbyMatchListReturnedMethod(LobbyMatchList_t callback) => LobbyMatchListReturnedEvent?.Invoke(callback);
        

        private readonly struct SteamEvent
        {
            private readonly object _targetObject;
            private readonly object _actionObject;
            private readonly MethodInfo _callMethod;

            public SteamEvent(object targetObject, object actionObject, MethodInfo callMethod)
            {
                _targetObject = targetObject;
                _actionObject = actionObject;
                _callMethod = callMethod;
            }

            public void CallEvent()
            {
                _callMethod.Invoke(_targetObject, new[] {_actionObject});
            }
        }
        
    }
}