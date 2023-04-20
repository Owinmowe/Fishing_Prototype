namespace FishingPrototype.Network.Messages
{
    public class MirrorServerMessageManager
    {
        public ServerMessageHandler<AuthenticateMessage> AuthenticationServerMessageHandler { get; }

        public MirrorServerMessageManager()
        {
            AuthenticationServerMessageHandler = new ServerMessageHandler<AuthenticateMessage>();
        }
    }
}
