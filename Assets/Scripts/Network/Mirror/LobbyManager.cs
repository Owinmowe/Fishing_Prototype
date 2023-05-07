using FishingPrototype.Gameplay.Boat;
using FishingPrototype.Network.Data;
using FishingPrototype.Player;
using UnityEngine;
using Mirror;

namespace FishingPrototype.Network.Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private NetworkBoat networkBoatPrefab;
        
        private void Start()
        {
            CustomNetworkManager.Instance.OnPlayerIdentify += CreatePlayerBoat;
        }

        private void OnDestroy()
        {
            CustomNetworkManager.Instance.OnPlayerIdentify -= CreatePlayerBoat;;
        }

        private void CreatePlayerBoat(PlayerReferences playerReferences, NetworkConnection conn = null)
        {
            NetworkBoat newBoat = Instantiate(networkBoatPrefab);

            if (conn != null) NetworkServer.Spawn(newBoat.gameObject, conn);
            else NetworkServer.Spawn(newBoat.gameObject);

            IPlayerDataControl playerDataControl = newBoat.GetComponent<IPlayerDataControl>();
            playerDataControl.SetPlayerData(playerReferences.playerData);

            playerReferences.playerGameObject = newBoat.BaseGameObject;
        }
    }
}
