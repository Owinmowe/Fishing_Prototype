using System.Collections.Generic;
using UnityEngine;

namespace FishingPrototype.Gameplay.Maps.Data
{
    [CreateAssetMenu(fileName = "Maps List", menuName = "Gameplay Data/Maps/Maps List", order = 1)]
    public class MapsList : ScriptableObject
    {
        public List<MapData> maps;
    }
}
