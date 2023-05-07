using FishingPrototype.Gameplay.FishingSpot.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FishingPrototype.Gameplay.Maps.Data
{
    [CreateAssetMenu(fileName = "Spawn Chance Data", menuName = "Gameplay Data/Maps/Spawn Chance Data", order = 1)]
    public class SpawnChanceData : ScriptableObject
    {
        public SpawnChances spawnChances;

        public void RollChance(ref FishingSpotData fishingSpotData)
        {
            int typeIndex = 0;
            int roll = Random.Range(0, 100);
            for (int i = 0; i < spawnChances.chanceTypeList.Count; i++)
            {
                if (spawnChances.chanceTypeList[i] > roll)
                {
                    typeIndex = i;
                    break;
                }
            }
            
            FishingSpotType fishingSpotType = (FishingSpotType)typeIndex;
            int minPossibleAmount = spawnChances.chanceAmountMinList[typeIndex];
            int maxPossibleAmount = spawnChances.chanceAmountMaxList[typeIndex];
            int amount = Random.Range(minPossibleAmount, maxPossibleAmount + 1);

            fishingSpotData.amount = amount;
            fishingSpotData.type = fishingSpotType;
        }
    }
}
