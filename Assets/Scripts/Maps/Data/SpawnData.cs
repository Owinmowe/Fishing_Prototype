using UnityEngine;

namespace FishingPrototype.Gameplay.Maps.Data
{
    [System.Serializable]
    public struct SpawnData
    {
        public Transform spawnPosition;
        public float spawnVarianceDistance;
        public SpawnChanceData spawnChanceData;
    }
}