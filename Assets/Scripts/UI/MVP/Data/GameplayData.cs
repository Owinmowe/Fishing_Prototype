using FishingPrototype.Gameplay.Minigames.Data;
using UnityEngine;

namespace FishingPrototype.MVP.Data
{
    [CreateAssetMenu(fileName = "Gameplay Data", menuName = "MVP/Gameplay Data", order = 1)]
    public class GameplayData : ScriptableData
    {
        public MiniGamesData miniGamesData;
    }
}