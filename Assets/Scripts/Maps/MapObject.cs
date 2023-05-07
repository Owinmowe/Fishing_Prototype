using System;
using System.Collections.Generic;
using FishingPrototype.Gameplay.FishingSpot.Data;
using FishingPrototype.Gameplay.Maps.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FishingPrototype.Gameplay.Maps
{
    public class MapObject : MonoBehaviour
    {
        [Header("Spawn Positions")]
        [SerializeField] private List<Transform> playersSpawnPosition;
        [SerializeField] private List<Transform> bossSpawnPositions;
        [SerializeField] private List<SpawnData> easyFishSpawnPositions;
        [SerializeField] private List<SpawnData> mediumFishSpawnPositions;
        [SerializeField] private List<SpawnData> hardFishSpawnPositions;
        
        public List<Transform> PlayersSpawnPosition => playersSpawnPosition;
        public List<Transform> BossSpawnPositions => bossSpawnPositions;
        public List<SpawnData> EasyFishSpawnPositions => easyFishSpawnPositions;
        public List<SpawnData> MediumFishSpawnPositions => mediumFishSpawnPositions;
        public List<SpawnData> HardFishSpawnPositions => hardFishSpawnPositions;

        public SpawnData GetRandomSpawnData(SpawnDifficulty difficulty, out int spawnIndex)
        {
            return difficulty switch
            {
                SpawnDifficulty.Easy => GetRandomSpawnDataFromList(easyFishSpawnPositions, out spawnIndex),
                SpawnDifficulty.Medium => GetRandomSpawnDataFromList(mediumFishSpawnPositions, out spawnIndex),
                SpawnDifficulty.Hard => GetRandomSpawnDataFromList(hardFishSpawnPositions, out spawnIndex),
                _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
            };
        }

        private SpawnData GetRandomSpawnDataFromList(List<SpawnData> spawnDataList, out int spawnIndex)
        {
            int index = Random.Range(0, spawnDataList.Count);
            spawnIndex = index;
            return spawnDataList[index];
        }
    }
}
