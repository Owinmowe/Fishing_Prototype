using UnityEngine;

namespace FishingPrototype.Gameplay.GameMode.Data
{
    [CreateAssetMenu(fileName = "GameMode Data", menuName = "GameMode/GameMode Data", order = 1)]
    public class GameModeData : ScriptableObject
    {
        public string gameModeName;
        public GameModeBase gameModeBase;
    }
}
