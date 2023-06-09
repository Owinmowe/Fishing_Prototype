using System.Collections.Generic;
using FishingPrototype.Network.Data;
using FishingPrototype.Network.Messages;
using FishingPrototype.Network.Steam;
using FishingPrototype.Player.Data;
using FishingPrototype.Utils;
using Mirror;
using Steamworks;

namespace FishingPrototype.Network
{
    public class CustomNetworkManager : NetworkManager
    {
        public event System.Action<PlayerReferences, int> OnPlayerConnect;
        public event System.Action<PlayerReferences, NetworkConnection> OnPlayerIdentify;
        public event System.Action<PlayerReferences, int> OnPlayerDisconnect;

        public event System.Action OnLobbyCreateOk;
        public event System.Action OnLobbyCreateFailed;
        public event System.Action<List<SteamLobbyData>> OnLobbiesGet;
        public event System.Action OnLobbyJoined;
        
        private const string HostNameKey = "HostName";
        private const string HostAddressKey = "HostAddress";
        private const string HostAvatarImageKey = "HostAvatar";

        private readonly Dictionary<int, PlayerReferences> _playersDictionary = new Dictionary<int, PlayerReferences>();
        private MirrorServerMessageManager _serverMessageManager;
        public static CustomNetworkManager Instance { get; private set; }
        
        public override void Awake()
        {
            base.Awake();
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            if (!SteamManager.Initialized)
                return;
            
            SteamEventDispatcher.LobbyCreatedEvent += OnLobbyCreated;
            SteamEventDispatcher.JoinRequestEvent += OnJoinRequest;
            SteamEventDispatcher.LobbyEnteredEvent += OnLobbyEntered;
            SteamEventDispatcher.LobbyMatchListReturnedEvent += OnLobbyMatchListReturned;
            
            _serverMessageManager = new MirrorServerMessageManager();
        }

        private void OnDisable()
        {
            SteamEventDispatcher.LobbyCreatedEvent -= OnLobbyCreated;
            SteamEventDispatcher.JoinRequestEvent -= OnJoinRequest;
            SteamEventDispatcher.LobbyEnteredEvent -= OnLobbyEntered;
            SteamEventDispatcher.LobbyMatchListReturnedEvent -= OnLobbyMatchListReturned;
        }

        public override void OnStartHost()
        {
            base.OnStartHost();
            _serverMessageManager.AuthenticationServerMessageHandler.BindHandler(OnClientAuthenticate);
            _serverMessageManager.AuthenticationServerMessageHandler.StartListening();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            CSteamID clientId = SteamUser.GetSteamID();
            AuthenticateMessage message = new AuthenticateMessage()
            {
                nickname = SteamFriends.GetPersonaName(),
                steamId = clientId.m_SteamID
            };
            NetworkClient.Send(message);
        }

        public override void OnStopHost()
        {
            base.OnStopHost();
            if(NetworkServer.active) _serverMessageManager.AuthenticationServerMessageHandler.StopListening();
        }
        
        private void OnClientAuthenticate(NetworkConnectionToClient conn, AuthenticateMessage message)
        {
            if (_playersDictionary.ContainsKey(conn.connectionId))
            {
                CSteamID playerId = new CSteamID(message.steamId);
                _playersDictionary[conn.connectionId].clientCSteamID = playerId;
                _playersDictionary[conn.connectionId].playerData = new PlayerData()
                {
                    nickname = message.nickname,
                    iImage = SteamFriends.GetMediumFriendAvatar(playerId)
                };
                OnPlayerIdentify?.Invoke(_playersDictionary[conn.connectionId], conn);
            }
        }
        
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            PlayerReferences playerReferences = new PlayerReferences();
            if (!_playersDictionary.ContainsKey(conn.connectionId))
            {
                _playersDictionary.Add(conn.connectionId, playerReferences);
                OnPlayerConnect?.Invoke(playerReferences, conn.connectionId);
            }
        }
        
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            if (_playersDictionary.ContainsKey(conn.connectionId))
            {
                OnPlayerDisconnect?.Invoke(_playersDictionary[conn.connectionId], conn.connectionId);
                _playersDictionary.Remove(conn.connectionId);
            }
        }

        public void RequestCreateLobby(bool publicLobby) //TODO Make possible all types of lobby
        {
            ELobbyType lobbyType = publicLobby ? ELobbyType.k_ELobbyTypePublic : ELobbyType.k_ELobbyTypeFriendsOnly;
            SteamMatchmaking.CreateLobby(lobbyType, maxConnections);
        }

        public void RequestJoinLobby(ulong lobbyId)
        {
            SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
        }

        public void RequestLobbiesList()
        {
            SteamMatchmaking.RequestLobbyList();
        }
        
        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                OnLobbyCreateFailed?.Invoke();
                return;
            }
            
            StartHost();
            SetLobbyExternalData(callback.m_ulSteamIDLobby);
            OnLobbyCreateOk?.Invoke();
        }

        private void SetLobbyExternalData(ulong steamIDLobby)
        {
            CSteamID lobbyId = new CSteamID(steamIDLobby);
            CSteamID hostId = SteamUser.GetSteamID();

            SteamMatchmaking.SetLobbyData(lobbyId, HostNameKey,SteamFriends.GetPersonaName());
            SteamMatchmaking.SetLobbyData(lobbyId, HostAvatarImageKey,SteamFriends.GetSmallFriendAvatar(hostId).ToString());
            SteamMatchmaking.SetLobbyData(lobbyId, HostAddressKey,hostId.ToString());
        }
        
        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if (NetworkServer.active) return;

            CSteamID lobbyCSteamID = new CSteamID(callback.m_ulSteamIDLobby);
            networkAddress = SteamMatchmaking.GetLobbyData(lobbyCSteamID, HostAddressKey);
            StartClient();
            OnLobbyJoined?.Invoke();
        }

        private void OnLobbyMatchListReturned(LobbyMatchList_t callback)
        {
            List<SteamLobbyData> lobbiesData = new List<SteamLobbyData>();
            for (int i = 0; i < callback.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
                CSteamID hostId = SteamMatchmaking.GetLobbyOwner(lobbyId);
                int iImage = SteamFriends.GetSmallFriendAvatar(hostId);
                
                SteamLobbyData steamLobbyData = new SteamLobbyData
                {
                    hostAddressKey = SteamMatchmaking.GetLobbyData(lobbyId, HostAddressKey),
                    hostName = SteamMatchmaking.GetLobbyData(lobbyId, HostNameKey),
                };
                
                steamLobbyData.hostAvatar = SteamUtilities.GetSteamImageAsTexture(iImage);
                steamLobbyData.SetTextureLoadCallback();
                
                lobbiesData.Add(steamLobbyData);
            }
            OnLobbiesGet?.Invoke(lobbiesData);
        }
    }
}
