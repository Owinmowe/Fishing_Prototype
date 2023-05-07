using System.Collections.Generic;
using UnityEngine;

namespace FishingPrototype.Gameplay.GameMode.Data
{
    [CreateAssetMenu(fileName = "GamesMode List", menuName = "Gameplay Data/GameMode/GamesMode List", order = 1)]
    public class GamesModeList : ScriptableObject
    {
        public List<GameModeData> gameModes;
    }
}