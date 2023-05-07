using FishingPrototype.Player;
using FishingPrototype.Player.Data;
using FishingPrototype.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FishingPrototype.Gameplay.Boat
{
    public class BoatUi : MonoBehaviour
    {
        [SerializeField] private GameObject baseBoatObject;
        [SerializeField] private TextMeshProUGUI playerNicknameText;
        [SerializeField] private RawImage playerAvatarImage;

        private IPlayerDataControl _playerDataControl;
        
        private void Awake()
        {
            _playerDataControl = baseBoatObject.GetComponent<IPlayerDataControl>();
            _playerDataControl.OnNewPlayerDataSet += OnPlayerDataSet;
        }

        private void OnDestroy()
        {
            _playerDataControl.OnNewPlayerDataSet -= OnPlayerDataSet;
        }

        private void OnPlayerDataSet(PlayerData data)
        {
            playerNicknameText.text = data.nickname;
            playerAvatarImage.texture = SteamUtilities.GetSteamImageAsTexture(data.iImage); //TODO Find a better alternative than call Steam Api from UI
        }
    }
}
