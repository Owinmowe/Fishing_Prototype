using Mirror;

namespace FishingPrototype.Network.Messages
{
    public struct AuthenticateMessage : NetworkMessage
    {
        public string nickname;
        public ulong steamId;
    }
}
