using System;
using FishingPrototype.Gameplay.FishingSpot;
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

        [SerializeField] private GameObject networkFishingSpotPrefab;
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
            SpawnTestFishingSpot();
            RpcStartGame();
        }

        [ClientRpc]
        private void RpcStartGame()
        {
            OnGameStarted?.Invoke();
        }

        private void SpawnTestFishingSpot()
        {
            GameObject go = Instantiate(networkFishingSpotPrefab);
            NetworkServer.Spawn(go);
            HostSetFishingSpot(go, FishingSpotType.KeyLogger, 5);
        }

        private void HostSetFishingSpot(GameObject go, FishingSpotType type, int amount)
        {
            SetFishingSpotLocal(go, type, amount);
            RpcSetFishingSpot(go, type, amount);
        }
        
        [ClientRpc]
        private void RpcSetFishingSpot(GameObject go, FishingSpotType type, int amount)
        {
            IFishingSpot fishingSpot = go.GetComponent<IFishingSpot>();
            fishingSpot.SetFishingSpot(type, amount);
        }

        private void SetFishingSpotLocal(GameObject go, FishingSpotType type, int amount)
        {
            IFishingSpot fishingSpot = go.GetComponent<IFishingSpot>();
            fishingSpot.SetFishingSpot(type, amount);
        }

    }
}