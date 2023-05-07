using System;
using System.Collections.Generic;
using FishingPrototype.Gameplay.Data;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.Maps.Data;
using FishingPrototype.Network.Data;
using UnityEngine;

namespace FishingPrototype.Gameplay.GameMode
{
    public abstract class GameModeBase : MonoBehaviour
    {
        protected Dictionary<ulong, PlayerReferences> _startingPlayersDictionary = new Dictionary<ulong, PlayerReferences>();
        protected Dictionary<ulong, PlayerReferences> _currentPlayersDictionary = new Dictionary<ulong, PlayerReferences>();
        public event Action<SpawnDifficulty> OnSpawnFishingSpot;
        public event Action OnSpawnBoss;
        public event Action<GameSessionReport> OnGameEnded;

        protected void CallGameEndedEvent(GameSessionReport report) => OnGameEnded?.Invoke(report);

        protected void CallSpawnFishingSpot(SpawnDifficulty difficulty) =>
            OnSpawnFishingSpot?.Invoke(difficulty);
        protected void CallSpawnBoss() => OnSpawnBoss?.Invoke();
        
        public void StartGame(Dictionary<ulong, PlayerReferences> players)
        {
            foreach (var playerPair in players)
            {
                _startingPlayersDictionary.Add(playerPair.Key, playerPair.Value);
                _currentPlayersDictionary.Add(playerPair.Key, playerPair.Value);
            }
            StartGameModeInternal();
        }

        protected abstract void StartGameModeInternal();
        
        public void RemovePlayer(ulong steamId)
        {
            _currentPlayersDictionary.Remove(steamId);
            RemovePlayerInternal();
        }

        protected abstract void RemovePlayerInternal();
    }
}