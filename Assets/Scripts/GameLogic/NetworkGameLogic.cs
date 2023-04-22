using System;
using FishingPrototype.Network;
using FishingPrototype.Network.Data;
using Mirror;
using UnityEngine;

namespace FishingPrototype.Gameplay.Logic
{
    public class NetworkGameLogic : NetworkBehaviour, IGameLogic
    {
        public Action OnGameStarted { get; set; }
        public Action OnGameEnded { get; set; }

        private readonly SyncDictionary<ulong, PlayerReferences> players = new SyncDictionary<ulong, PlayerReferences>();

        private void Start()
        {
            CustomNetworkManager.Instance.OnPlayerIdentify += AddPlayer;
            CustomNetworkManager.Instance.OnPlayerDisconnect += RemovePlayer;
            IGameLogic.OnGameLogicSet?.Invoke(this);
        }

        private void OnDestroy()
        {
            CustomNetworkManager.Instance.OnPlayerIdentify -= AddPlayer;
            CustomNetworkManager.Instance.OnPlayerDisconnect -= RemovePlayer;
        }

        private void AddPlayer(PlayerReferences references, NetworkConnection connection)
        {
            players.Add(references.clientCSteamID.m_SteamID, references);
        }

        private void RemovePlayer(PlayerReferences references)
        {
            players.Remove(references.clientCSteamID.m_SteamID);
        }
        
        public void StartGame()
        {
            OnGameStarted?.Invoke();
            RpcStartGame();
        }

        [ClientRpc]
        private void RpcStartGame()
        {
            OnGameStarted?.Invoke();
        }

    }
}