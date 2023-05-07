using System;
using System.Collections.Generic;
using System.Linq;
using FishingPrototype.Gameplay.Data;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.GameMode;
using FishingPrototype.Gameplay.GameMode.Data;
using FishingPrototype.Gameplay.Maps;
using FishingPrototype.Gameplay.Maps.Data;
using FishingPrototype.Network;
using FishingPrototype.Network.Data;
using FishingPrototype.Waves;
using Mirror;
using UnityEngine;

namespace FishingPrototype.Gameplay.Logic
{
    public class NetworkGameLogic : NetworkBehaviour, IGameLogic
    {
        public Action OnGameStarted { get; set; }
        public Action OnGameEnded { get; set; }

        [SerializeField] private NetworkFishingSpot networkFishingSpot;
        
        private readonly SyncDictionary<ulong, PlayerReferences> _syncPlayers = new SyncDictionary<ulong, PlayerReferences>();

        private GameModeBase _gameModeBase;
        private MapObject _mapObject;
        
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
            _syncPlayers.Add(references.clientCSteamID.m_SteamID, references);
        }

        private void RemovePlayer(PlayerReferences references)
        {
            _syncPlayers.Remove(references.clientCSteamID.m_SteamID);
            if(_gameModeBase != null) _gameModeBase.RemovePlayer(references.clientCSteamID.m_SteamID);
        }
        
        public void InitializeGame(GameModeData gameModeData, MapData mapData)
        {
            SetGameMode(gameModeData);
            SetMap(mapData);
            StartGame();
        }

        private void SetGameMode(GameModeData data)
        {
            _gameModeBase = Instantiate(data.gameModeBase, transform);
            _gameModeBase.OnSpawnFishingSpot += NetworkSpawnFishingSpot;
            _gameModeBase.OnSpawnBoss += SpawnBoss;
            _gameModeBase.OnGameEnded += GameEnded;
        }

        private void SetMap(MapData data)
        {
            _mapObject = Instantiate(data.mapObject, transform);
            WaveManager.Get().ChangeMaterial(data.waterMaterial, data.waveConfiguration);
        }

        private void StartGame()
        {
            Dictionary<ulong, PlayerReferences> playersDictionary = _syncPlayers.ToDictionary(playerPair => playerPair.Key, playerPair => playerPair.Value);
            _gameModeBase.StartGame(playersDictionary);
            OnGameStarted?.Invoke();
            RpcStartGame();
        }
        
        private void RemoveGameMode()
        {
            _gameModeBase.OnGameEnded -= GameEnded;
            Destroy(_gameModeBase);
            _gameModeBase = null;
        }

        private void RemoveMap()
        {
            Destroy(_mapObject);
            _mapObject = null;
        }
        
        [ClientRpc]
        private void RpcStartGame()
        {
            OnGameStarted?.Invoke();
        }
        
        private void NetworkSpawnFishingSpot(SpawnDifficulty difficulty)
        {
            SpawnData spawnData = _mapObject.GetRandomSpawnData(difficulty);
            Tuple<FishingSpotType, int> fishingSpotTuple = spawnData.spawnChanceData.RollChance();
            IFishingSpot fishingSpot = Instantiate(networkFishingSpot, spawnData.spawnPosition);
            NetworkServer.Spawn(fishingSpot.BaseGameObject);
            HostSetFishingSpot(fishingSpot.BaseGameObject, fishingSpotTuple.Item1, fishingSpotTuple.Item2);
            RpcSetFishingSpot(fishingSpot.BaseGameObject, fishingSpotTuple.Item1, fishingSpotTuple.Item2);
        }

        [ClientRpc]
        private void RpcSetFishingSpot(GameObject go, FishingSpotType type, int amount)
        {
            IFishingSpot fishingSpot = go.GetComponent<IFishingSpot>();
            fishingSpot.SetFishingSpot(type, amount);
        }

        private void HostSetFishingSpot(GameObject go, FishingSpotType type, int amount)
        {
            IFishingSpot fishingSpot = go.GetComponent<IFishingSpot>();
            fishingSpot.SetFishingSpot(type, amount);
            fishingSpot.OnFishingSpotEmpty += OnFishingSpotEmpty;
        }

        private void SpawnBoss()
        {
            
        }
        
        private void OnFishingSpotEmpty(FishingSpotType fishingSpotType)
        {
            
        }
        
        private void GameEnded(GameSessionReport gameSessionReport)
        {
            RemoveGameMode();
            RemoveMap();
            OnGameEnded?.Invoke();
        }
        
    }
}