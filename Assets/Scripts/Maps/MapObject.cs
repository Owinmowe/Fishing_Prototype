using System;
using System.Collections.Generic;
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

        public SpawnData GetRandomSpawnData(SpawnDifficulty difficulty)
        {
            return difficulty switch
            {
                SpawnDifficulty.Easy => GetRandomSpawnDataFromList(easyFishSpawnPositions),
                SpawnDifficulty.Medium => GetRandomSpawnDataFromList(mediumFishSpawnPositions),
                SpawnDifficulty.Hard => GetRandomSpawnDataFromList(hardFishSpawnPositions),
                _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
            };
        }

        private SpawnData GetRandomSpawnDataFromList(List<SpawnData> spawnDataList)
        {
            int index = Random.Range(0, spawnDataList.Count);
            return spawnDataList[index];
        }
    }
}
