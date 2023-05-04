using System.Collections.Generic;
using UnityEngine;

namespace FishingPrototype.Gameplay.Maps.Data
{
    [System.Serializable]
    public struct SpawnChances
    {
        [HideInInspector] public List<float> chanceTypeList;
        [HideInInspector] public List<int> chanceAmountMinList;
        [HideInInspector] public List<int> chanceAmountMaxList;
    }
}