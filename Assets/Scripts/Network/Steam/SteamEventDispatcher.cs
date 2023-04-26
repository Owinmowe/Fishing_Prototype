using System;
using System.Collections.Generic;
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
            SteamEvent steamEvent = new SteamEvent(callAction: LobbyCreatedEvent, actionObject: callback);
            _steamEventsQueue.Enqueue(steamEvent);
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            SteamEvent steamEvent = new SteamEvent(callAction: JoinRequestEvent, actionObject: callback);
            _steamEventsQueue.Enqueue(steamEvent);
        }
        
        
        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            SteamEvent steamEvent = new SteamEvent(callAction: LobbyEnteredEvent, actionObject: callback);
            _steamEventsQueue.Enqueue(steamEvent);
        }

        private void OnLobbyMatchListReturned(LobbyMatchList_t callback)
        {
            SteamEvent steamEvent = new SteamEvent(callAction: LobbyMatchListReturnedEvent, actionObject: callback);
            _steamEventsQueue.Enqueue(steamEvent);
        }
        

        private readonly struct SteamEvent
        {
            private readonly object _actionObject;
            private readonly Delegate _callAction;

            public SteamEvent(object actionObject, Delegate callAction)
            {
                _actionObject = actionObject;
                _callAction = callAction;
            }

            public void CallEvent()
            {
                _callAction.DynamicInvoke(new[] { _actionObject });
            }
        }
        
    }
}