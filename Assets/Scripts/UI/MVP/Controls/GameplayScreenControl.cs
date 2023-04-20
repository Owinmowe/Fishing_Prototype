using UnityEngine;

namespace FishingPrototype.MVP.Control
{
    public class GameplayScreenControl : MonoBehaviour, IScreenControl
    {
        public void OpenScreen() => gameObject.SetActive(true);

        public void CloseScreen() => gameObject.SetActive(false);
    }
}