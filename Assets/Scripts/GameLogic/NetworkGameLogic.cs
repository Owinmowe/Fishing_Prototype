using System;
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
        
        private readonly SyncDictionary<ulong, PlayerReferences> players = new SyncDictionary<ulong, PlayerReferences>();

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
            players.Add(references.clientCSteamID.m_SteamID, references);
        }

        private void RemovePlayer(PlayerReferences references)
        {
            players.Remove(references.clientCSteamID.m_SteamID);
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
            _gameModeBase.SetSpawnMethod(NetworkSpawnMethod);
            _gameModeBase.SetFishingSpotMethod(NetworkSetFishingSpot);
            
            _gameModeBase.OnGameEnded += GameEnded;
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

        private void NetworkSpawnMethod(GameObject gObject)
        {
            NetworkServer.Spawn(gObject);
        }

        private void NetworkSetFishingSpot(GameObject go, FishingSpotType type, int amount)
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

        private void GameEnded(GameSessionReport gameSessionReport)
        {
            RemoveGameMode();
            OnGameEnded?.Invoke();
        }
        
    }
}