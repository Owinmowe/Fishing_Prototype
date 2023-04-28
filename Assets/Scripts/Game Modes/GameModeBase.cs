using System;
using System.Collections.Generic;
using FishingPrototype.Gameplay.Data;
using FishingPrototype.Network.Data;
using UnityEngine;

namespace FishingPrototype.Gameplay.GameMode
{
    public abstract class GameModeBase : MonoBehaviour
    {
        protected List<PlayerReferences> playerReferences = new List<PlayerReferences>();
        protected Action<GameObject> spawnMethod;
        protected Action<GameObject, FishingSpotType, int> fishingSpotSetMethod;
        public event Action<GameSessionReport> OnGameEnded;

        protected void CallGameEndedEvent(GameSessionReport report) => OnGameEnded?.Invoke(report);
        public virtual void StartGame(List<PlayerReferences> players) => playerReferences = players;
        public virtual void RemovePlayer(PlayerReferences player) => playerReferences.Remove(player);
        public virtual void SetSpawnMethod(Action<GameObject> newSpawnMethod) => spawnMethod = newSpawnMethod;
        public virtual void SetFishingSpotMethod(Action<GameObject, FishingSpotType, int> newFishingSpotMethod) =>
            fishingSpotSetMethod = newFishingSpotMethod;
    }
}