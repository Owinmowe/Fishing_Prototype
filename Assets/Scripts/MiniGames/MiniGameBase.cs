using System;
using FishingPrototype.Gameplay.FishingSpot;
using UnityEngine;

namespace FishingPrototype.Gameplay.Minigames
{
    public abstract class MiniGameBase : MonoBehaviour
    {
        public event Action OnMiniGameComplete;
        [SerializeField] private FishingSpotType miniGameType;
        protected void CallMiniGameCompleteEvent() => OnMiniGameComplete?.Invoke();
        public FishingSpotType MiniGameType => miniGameType;
        public abstract void StartMiniGame(IFishingSpot fishingSpot);
        public abstract void CloseMiniGame();
        public abstract void ReceiveMiniGameInput1();
        public abstract void ReceiveMiniGameInput2();
    }
}

