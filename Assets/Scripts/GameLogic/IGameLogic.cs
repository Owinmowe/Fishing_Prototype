using FishingPrototype.Gameplay.GameMode;
using FishingPrototype.Gameplay.GameMode.Data;
using FishingPrototype.Gameplay.Maps.Data;
using UnityEngine;

namespace FishingPrototype.Gameplay.Logic
{
    public interface IGameLogic
    {
        public static System.Action<IGameLogic> OnGameLogicSet;
        public System.Action OnGameStarted { get; set; }
        public System.Action OnGameEnded { get; set; }
        public void InitializeGame(GameModeData gameModeData, MapData mapData);
    }
}
