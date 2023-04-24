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
        public abstract void ReceiveMiniGameInput1();
        public abstract void ReceiveMiniGameInput2();
    }
}

