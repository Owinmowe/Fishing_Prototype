using UnityEngine;


namespace FishingPrototype.Test
{
    public class TestWindowsSizeChanger : MonoBehaviour
    {

        [Header("Window Configuration")] 
        [SerializeField] private bool fullscreen;
        [SerializeField] private Vector2Int pixelsAmount;
        
        private void Awake()
        {
            Screen.SetResolution(pixelsAmount.x, pixelsAmount.y, fullscreen);
        }
    }
}
