using System;
using System.Collections.Generic;
using System.Linq;
using FishingPrototype.Gameplay.Data;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.GameMode;
using FishingPrototype.Gameplay.GameMode.Data;
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

        [SerializeField] private NetworkFishingSpot networkFishingSpot;
        
        private readonly SyncDictionary<ulong, PlayerReferences> syncPlayers = new SyncDictionary<ulong, PlayerReferences>();

        private GameModeBase _gameModeBase;
        
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
            syncPlayers.Add(references.clientCSteamID.m_SteamID, references);
        }

        private void RemovePlayer(PlayerReferences references)
        {
            syncPlayers.Remove(references.clientCSteamID.m_SteamID);
            if(_gameModeBase != null) _gameModeBase.RemovePlayer(references.clientCSteamID.m_SteamID);
        }
        
        public void StartGame(GameModeData gameModeData)
        {
            SetGameMode(gameModeData.gameModeBase);
            
            OnGameStarted?.Invoke();
            RpcStartGame();
        }

        private void SetGameMode(GameModeBase gameModeBase)
        {
            _gameModeBase = Instantiate(gameModeBase);
            _gameModeBase.SetFishingSpotSpawnMethod(NetworkSetFishingSpot);
            
            _gameModeBase.OnGameEnded += GameEnded;

            Dictionary<ulong, PlayerReferences> playersDictionary = syncPlayers.ToDictionary(playerPair => playerPair.Key, playerPair => playerPair.Value);
            _gameModeBase.StartGame(playersDictionary);
        }

        private void RemoveGameMode()
        {
            _gameModeBase.OnGameEnded -= GameEnded;
            _gameModeBase = null;
        }
        
        [ClientRpc]
        private void RpcStartGame()
        {
            OnGameStarted?.Invoke();
        }
        
        private IFishingSpot NetworkSetFishingSpot(FishingSpotType type, int amount)
        {
            IFishingSpot fishingSpot = Instantiate(networkFishingSpot);
            NetworkServer.Spawn(fishingSpot.BaseGameObject);
            SetFishingSpotLocal(fishingSpot.BaseGameObject, type, amount);
            RpcSetFishingSpot(fishingSpot.BaseGameObject, type, amount);
            return fishingSpot;
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

        private void GameEnded(GameSessionReport gameSessionReport)
        {
            RemoveGameMode();
            OnGameEnded?.Invoke();
        }
        
    }
}