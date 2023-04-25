using System;
using FishingPrototype.Gameplay.FishingSpot;
using UnityEngine;

namespace FishingPrototype.Gameplay.Minigames
{
    public abstract class MiniGameBase : MonoBehaviour
    {
        public event Action OnMiniGameComplete;
        protected void CallMiniGameCompleteEvent() => OnMiniGameComplete?.Invoke();
        public abstract FishingSpotType GetMiniGameType();
        public abstract void StartMiniGame(IFishingSpot fishingSpot);
        public abstract void CloseMiniGame();
        public abstract void PerformMiniGameInput1();
        public abstract void PerformMiniGameInput2();
        public abstract void CancelMiniGameInput1();
        public abstract void CancelMiniGameInput2();
    }
}

