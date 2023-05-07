using FishingPrototype.Gameplay.GameMode.Data;
using FishingPrototype.Gameplay.Maps.Data;
using UnityEngine;

namespace FishingPrototype.MVP.Data
{
    [CreateAssetMenu(fileName = "Lobby Data", menuName = "MVP/Lobby Data", order = 1)]
    public class LobbyData : ScriptableData
    {
        public GamesModeList gamesModeList;
        public MapsList mapsList;
    }
}