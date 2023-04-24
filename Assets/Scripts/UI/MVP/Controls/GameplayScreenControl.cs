using System;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.Minigames;
using UnityEngine;

namespace FishingPrototype.MVP.Control
{
    public class GameplayScreenControl : MonoBehaviour, IScreenControl
    {
        [SerializeField] private FishingMiniGameControl fishingMiniGameControl;

        public Action OnFishingCompleted;

        private void Awake()
        {
            fishingMiniGameControl.OnFishingCompleted += OnFishingCompleted;
        }

        private void OnDestroy()
        {
            fishingMiniGameControl.OnFishingCompleted -= OnFishingCompleted;
        }

        public void OpenScreen() => gameObject.SetActive(true);
        public void CloseScreen() => gameObject.SetActive(false);

        public void InjectMiniGames(MiniGameBase[] miniGames) => fishingMiniGameControl.SetAllMiniGames(miniGames);
        public void PerformCustomInput1() => fishingMiniGameControl.ReceiveInput1();
        public void PerformCustomInput2() => fishingMiniGameControl.ReceiveInput2();
        public void FishingStarted(IFishingSpot fishingSpot) => fishingMiniGameControl.StartFishing(fishingSpot);
        public void FishingCanceled() => fishingMiniGameControl.CancelFishing();
        public void FishingFailed() => fishingMiniGameControl.FailFishing();

    }
}