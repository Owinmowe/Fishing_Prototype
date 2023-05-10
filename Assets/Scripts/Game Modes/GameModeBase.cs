using System;
using System.Collections.Generic;
using FishingPrototype.Gameplay.Data;
using FishingPrototype.Gameplay.FishingSpot.Data;
using FishingPrototype.Gameplay.Maps.Data;
using FishingPrototype.Network.Data;
using UnityEngine;

namespace FishingPrototype.Gameplay.GameMode
{
    public abstract class GameModeBase : MonoBehaviour
    {
        protected Dictionary<int, PlayerReferences> _startingPlayersDictionary = new Dictionary<int, PlayerReferences>();
        protected Dictionary<int, PlayerReferences> _currentPlayersDictionary = new Dictionary<int, PlayerReferences>();
        public event Action<SpawnDifficulty> OnSpawnFishingSpot;
        public event Action<FishingSpotData> OnSpawnFishingSpotWithData;
        public event Action OnChangeMap;
        public event Action OnSpawnAllFishingSpot;
        public event Action OnSpawnBoss;
        public event Action<GameSessionReport> OnGameEnded;

        protected void CallGameEndedEvent(GameSessionReport report) => OnGameEnded?.Invoke(report);
        protected void CallSpawnFishingSpot(SpawnDifficulty difficulty) =>
            OnSpawnFishingSpot?.Invoke(difficulty);
        protected void CallSpawnFishingSpotWithData(FishingSpotData fishingSpotData) =>
            OnSpawnFishingSpotWithData?.Invoke(fishingSpotData);
        protected void CallSpawnAllFishingSpot() => OnSpawnAllFishingSpot?.Invoke();
        protected void CallSpawnBoss() => OnSpawnBoss?.Invoke();
        protected void CallChangeMap() => OnChangeMap?.Invoke();
        
        public void StartGame(Dictionary<int, PlayerReferences> players)
        {
            foreach (var playerPair in players)
            {
                _startingPlayersDictionary.Add(playerPair.Key, playerPair.Value);
                _currentPlayersDictionary.Add(playerPair.Key, playerPair.Value);
            }
            StartGameModeInternal();
        }

        protected abstract void StartGameModeInternal();
        
        public void RemovePlayer(int connectionId)
        {
            _currentPlayersDictionary.Remove(connectionId);
            RemovePlayerInternal();
        }

        protected abstract void RemovePlayerInternal();

        public virtual void OnFishingSpotEmpty(FishingSpotData fishingSpotData)
        {
            OnSpawnFishingSpotWithData?.Invoke(fishingSpotData);
        }
    }
}