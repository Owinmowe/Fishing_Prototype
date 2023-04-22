using FishingPrototype.Utils;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FishingPrototype.UI
{
    public class ConnectedPanelUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNicknameText;
        [SerializeField] private RawImage playerAvatarImage;

        public void SetPanelState(bool panelActive)
        {
            gameObject.SetActive(panelActive);
            CmdSetPanelData(panelActive);
        }

        [ClientRpc]
        private void CmdSetPanelData(bool panelActive)
        {
            gameObject.SetActive(panelActive);
        }
        
        public void SetPanelData(string playerNickname, int iImage)
        {
            playerNicknameText.text = playerNickname;
            playerAvatarImage.texture = SteamUtilities.GetSteamImageAsTexture(iImage); //TODO Find a better alternative than call Steam Api from UI
            CmdSetPanelData(playerNickname, iImage);
        }

        [ClientRpc]
        private void CmdSetPanelData(string playerNickname, int iImage)
        {
            playerNicknameText.text = playerNickname;
            playerAvatarImage.texture = SteamUtilities.GetSteamImageAsTexture(iImage); //TODO Find a better alternative than call Steam Api from UI
        }
    }
}
