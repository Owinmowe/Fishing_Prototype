using UnityEngine;

namespace FishingPrototype.Gameplay.Maps.Data
{
    [CreateAssetMenu(fileName = "Spawn Position Data", menuName = "Gameplay Data/Maps/Spawn Position Data", order = 1)]
    public class SpawnData : ScriptableObject
    {
        public SpawnChances spawnChances;
    }
}
