using System;
using Mirror;

namespace FishingPrototype.Network.Messages
{
    public class ServerMessageHandler<T> where T : struct, NetworkMessage
    {
        private Action<NetworkConnectionToClient, T> MessageReceivedEvent;

        public void BindHandler(Action<NetworkConnectionToClient, T> action)
        {
            MessageReceivedEvent = action;
        }

        public void StartListening()
        {
            NetworkServer.RegisterHandler(MessageReceivedEvent, false);
        }

        public void StopListening()
        {
            NetworkServer.UnregisterHandler<T>();
        }
    }
}
