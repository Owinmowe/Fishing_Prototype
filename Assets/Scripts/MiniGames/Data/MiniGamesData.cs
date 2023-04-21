using System;
using UnityEngine;

namespace FishingPrototype.Gameplay.Minigames.Data
{
    [CreateAssetMenu(fileName = "MiniGames Data", menuName = "MiniGames/Data", order = 1)]
    public class MiniGamesData : ScriptableObject
    {
        [HideInInspector] public MiniGameBase[] miniGamesPrefabs = new MiniGameBase[Enum.GetValues(typeof(FishingSpotType)).Length];

        public MiniGameBase[] GetAllMiniGames => miniGamesPrefabs;
        
        public MiniGameBase GetMiniGamePrefab(FishingSpotType type)
        {
            return miniGamesPrefabs[(int)type];
        }
    }
}
