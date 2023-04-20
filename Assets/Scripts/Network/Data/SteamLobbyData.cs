using FishingPrototype.Utils;
using Steamworks;
using UnityEngine;

namespace FishingPrototype.Network.Data
{
    public struct SteamLobbyData
    {
        public string hostName;
        public string hostAddressKey;
        public Texture2D hostAvatar;

        public void SetTextureLoadCallback()
        {
            Callback<AvatarImageLoaded_t>.Create(ChangeAvatarImage);
        }

        private void ChangeAvatarImage(AvatarImageLoaded_t callback)
        {
            hostAvatar = SteamUtilities.GetSteamImageAsTexture(callback.m_iImage);
        }
        
    }
}
