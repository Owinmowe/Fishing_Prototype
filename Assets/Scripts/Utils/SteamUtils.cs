using Steamworks;
using UnityEngine;

namespace FishingPrototype.Utils
{
    public class SteamUtilities : MonoBehaviour
    {
        public static Texture2D GetSteamImageAsTexture(int iImage)
        {
            Texture2D texture2D = null;

            if (SteamUtils.GetImageSize(iImage, out uint width, out uint height))
            {
                uint bufferSize = width * height * 4;
                byte[] imageBuffer = new byte[bufferSize];

                if (SteamUtils.GetImageRGBA(iImage, imageBuffer, (int)bufferSize))
                {
                    texture2D = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                    texture2D.LoadRawTextureData(imageBuffer);
                    texture2D.Apply();
                }
            }
            
            return texture2D;
        }
    }
}