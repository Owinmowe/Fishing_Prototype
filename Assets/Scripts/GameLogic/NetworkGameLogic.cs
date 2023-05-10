using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishingPrototype.Gameplay.Boat;
using FishingPrototype.Gameplay.Data;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.FishingSpot.Data;
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
        
        private readonly SyncDictionary<int, PlayerReferences> _syncPlayers = new SyncDictionary<int, PlayerReferences>();

        private GameModeBase _gameModeBase;
        
        private MapObject _mapObject;
        private MapData _mapData;
        
        private void Awake()
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
            _syncPlayers.Add(connection.connectionId, references);
        }

        private void RemovePlayer(PlayerReferences references, int connectionId)
        {
            _syncPlayers.Remove(connectionId);
            if(_gameModeBase != null) _gameModeBase.RemovePlayer(connectionId);
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
            _gameModeBase.OnChangeMap += ChangeMap;
            _gameModeBase.OnSpawnFishingSpot += NetworkSpawnFishingSpot;
            _gameModeBase.OnSpawnFishingSpotWithData += NetworkSpawnFishingSpotWithData;
            _gameModeBase.OnSpawnAllFishingSpot += NetworkSpawnAllFishingSpots;
            _gameModeBase.OnSpawnBoss += SpawnBoss;
            _gameModeBase.OnGameEnded += GameEnded;
        }

        private void SetMap(MapData data)
        {
            _mapData = data;
        }

        private void StartGame()
        {
            Dictionary<int, PlayerReferences> playersDictionary = _syncPlayers.ToDictionary(playerPair => playerPair.Key, playerPair => playerPair.Value);
            _gameModeBase.StartGame(playersDictionary);
            OnGameStarted?.Invoke();
            RpcStartGame();
        }
        
        private void RemoveGameMode()
        {
            _gameModeBase.OnChangeMap -= ChangeMap;
            _gameModeBase.OnSpawnFishingSpot -= NetworkSpawnFishingSpot;
            _gameModeBase.OnSpawnFishingSpotWithData -= NetworkSpawnFishingSpotWithData;
            _gameModeBase.OnSpawnAllFishingSpot -= NetworkSpawnAllFishingSpots;
            _gameModeBase.OnSpawnBoss -= SpawnBoss;
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

        private async void ChangeMap()
        {
            _mapObject = Instantiate(_mapData.mapObject, transform);

            List<ITeleportComponent> teleportComponents = new List<ITeleportComponent>();
            foreach (var playerPair in _syncPlayers)
            {
                teleportComponents.Add(playerPair.Value.playerGameObject.GetComponent<ITeleportComponent>());
            }

            Task[] tasks = new Task[teleportComponents.Count];
            for (int i = 0; i < teleportComponents.Count; i++)
            {
                tasks[i] = teleportComponents[i].Dissolve();
            }

            await Task.WhenAll(tasks);
            
            NetworkSpawnAllFishingSpots();
            
            await WaveManager.Get().ChangeMaterial(_mapData.waterMaterial, _mapData.waveConfiguration);
            
            SendAllPlayersToSpawnPositions();
            
            foreach (var teleportComponent in teleportComponents)
            {
                teleportComponent.UnDissolve();
            }
        }

        private void NetworkSpawnAllFishingSpots()
        {
            NetworkSpawnAllFishingSpotsGeneric(_mapObject.EasyFishSpawnPositions.Count, SpawnDifficulty.Easy);
            NetworkSpawnAllFishingSpotsGeneric(_mapObject.MediumFishSpawnPositions.Count, SpawnDifficulty.Medium);
            NetworkSpawnAllFishingSpotsGeneric(_mapObject.HardFishSpawnPositions.Count, SpawnDifficulty.Hard);
        }

        private void NetworkSpawnAllFishingSpotsGeneric(int amount, SpawnDifficulty difficulty)
        {
            for (int i = 0; i < amount; i++)
            {
                FishingSpotData fishingSpotData = new FishingSpotData
                {
                    spawnDifficulty = difficulty
                };
                SpawnData spawnData = _mapObject.GetSpecificSpawnData(difficulty, i);
                spawnData.spawnChanceData.RollChance(ref fishingSpotData);
                SpawnFishingSpot(spawnData, fishingSpotData);
            }
        }

        private void NetworkSpawnFishingSpotWithData(FishingSpotData fishingSpotData)
        {
            SpawnData spawnData = _mapObject.GetSpecificSpawnData(fishingSpotData.spawnDifficulty, fishingSpotData.spawnIndex);
            spawnData.spawnChanceData.RollChance(ref fishingSpotData);
            SpawnFishingSpot(spawnData, fishingSpotData);
        }
        
        private void NetworkSpawnFishingSpot(SpawnDifficulty difficulty)
        {
            FishingSpotData fishingSpotData = new FishingSpotData
            {
                spawnDifficulty = difficulty
            };
            SpawnData spawnData = _mapObject.GetRandomSpawnData(difficulty, out fishingSpotData.spawnIndex);
            spawnData.spawnChanceData.RollChance(ref fishingSpotData);
            SpawnFishingSpot(spawnData, fishingSpotData);
        }
        
        private void SpawnFishingSpot(SpawnData spawnData, FishingSpotData fishingSpotData)
        {
            IFishingSpot fishingSpot = Instantiate(networkFishingSpot, spawnData.spawnPosition);
            NetworkServer.Spawn(fishingSpot.BaseGameObject);
            HostSetFishingSpot(fishingSpot.BaseGameObject, fishingSpotData);
            RpcSetFishingSpot(fishingSpot.BaseGameObject, fishingSpotData);
        }

        [ClientRpc]
        private void RpcSetFishingSpot(GameObject go, FishingSpotData fishingSpotData)
        {
            IFishingSpot fishingSpot = go.GetComponent<IFishingSpot>();
            fishingSpot.SetFishingSpot(fishingSpotData);
        }

        private void HostSetFishingSpot(GameObject go, FishingSpotData fishingSpotData)
        {
            IFishingSpot fishingSpot = go.GetComponent<IFishingSpot>();
            fishingSpot.SetFishingSpot(fishingSpotData);
            fishingSpot.OnFishingSpotEmpty += OnFishingSpotEmpty;
        }

        private void SpawnBoss()
        {
            
        }
        
        private void OnFishingSpotEmpty(FishingSpotData fishingSpotData)
        {
            _gameModeBase.OnFishingSpotEmpty(fishingSpotData);
        }

        private void SendAllPlayersToSpawnPositions()
        {
            int index = 0;
            foreach (var playerPair in _syncPlayers)
            {
                playerPair.Value.playerGameObject.transform.position = _mapObject.PlayersSpawnPosition[index].position;
                index++;
            }
        }
        
        private void DissolveAllPlayers()
        {
            foreach (var playerPair in _syncPlayers)
            {
                playerPair.Value.playerGameObject.GetComponent<ITeleportComponent>().Dissolve();
            }
        }

        private void UnDissolveAllPlayers()
        {
            foreach (var playerPair in _syncPlayers)
            {
                playerPair.Value.playerGameObject.GetComponent<ITeleportComponent>().UnDissolve();
            }
        }
        
        private void GameEnded(GameSessionReport gameSessionReport)
        {
            RemoveGameMode();
            RemoveMap();
            OnGameEnded?.Invoke();
        }
        
    }
}