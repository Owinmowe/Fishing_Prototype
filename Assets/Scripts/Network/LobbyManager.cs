using FishingPrototype.Gameplay.Boat;
using UnityEngine;
using Mirror;

namespace FishingPrototype.Network.Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private CustomNetworkManager networkManager;
        [SerializeField] private NetworkBoat networkBoatPrefab;

        private void Awake()
        {
            networkManager.OnPlayerIdentify += CreatePlayerBoat;
        }

        private void OnDestroy()
        {
            networkManager.OnPlayerIdentify -= CreatePlayerBoat;
        }

        private void CreatePlayerBoat(CustomNetworkManager.PlayerReferences playerReferences, NetworkConnection conn = null)
        {
            NetworkBoat newBoat = Instantiate(networkBoatPrefab);

            if (conn != null) NetworkServer.Spawn(newBoat.gameObject, conn);
            else NetworkServer.Spawn(newBoat.gameObject);

            IPlayerDataControl playerDataControl = newBoat.GetComponent<IPlayerDataControl>();
            playerDataControl.SetPlayerData(playerReferences.playerData);
        }
    }
}
