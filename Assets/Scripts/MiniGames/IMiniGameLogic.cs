using System;
using FishingPrototype.Gameplay.FishingSpot;

namespace FishingPrototype.Gameplay.Minigames
{
    public interface IMiniGameLogic
    {
        public event Action OnMiniGameComplete;
        public FishingSpotType MiniGameType { get; }
        public void StartMiniGame(IFishingSpot fishingSpot);
        public void CloseMiniGame();
        public void ReceiveMiniGameInput1();
        public void ReceiveMiniGameInput2();
    }
}

