using FishingPrototype.Gameplay.Boat;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.Minigames;
using FishingPrototype.MVP.Data;
using FishingPrototype.MVP.View;
using UnityEngine.InputSystem;

namespace FishingPrototype.MVP.Presenter
{
    public class GameplayPresenter: Presenter<GameView>
    {
        private GameplayData _scriptableData;
        private IBoat _localBoat;

        public GameplayPresenter(GameView view, GameplayData scriptableData) : base(view)
        {
            _scriptableData = scriptableData;
            InjectMiniGames(scriptableData.miniGamesData.miniGamesPrefabs);
        }

        protected override void AddViewListeners()
        {
            view.OnPerformedCustomInput1Event += OnPerformedCustomInput1;
            view.OnPerformedCustomInput2Event += OnPerformedCustomInput2;
            view.OnLocalBoatSetEvent += OnLocalBoatSet;
            view.OnLocalBoatRemoveEvent += OnLocalBoatRemove;
            view.FishingActionStartedEvent += OnFishingActionStarted;
            view.FishingActionCanceledEvent += OnFishingActionCanceled;
            view.FishingActionFailedEvent += OnFishingActionFailed;
            view.FishingActionCompletedEvent += OnFishingActionCompleted;
        }

        protected override void RemoveViewListeners()
        {
            view.OnPerformedCustomInput1Event -= OnPerformedCustomInput1;
            view.OnPerformedCustomInput2Event -= OnPerformedCustomInput2;
            view.OnLocalBoatSetEvent -= OnLocalBoatSet;
            view.OnLocalBoatRemoveEvent -= OnLocalBoatRemove;
            view.FishingActionStartedEvent -= OnFishingActionStarted;
            view.FishingActionCanceledEvent -= OnFishingActionCanceled;
            view.FishingActionFailedEvent -= OnFishingActionFailed;
            view.FishingActionCompletedEvent -= OnFishingActionCompleted;

        }

        private void InjectMiniGames(MiniGameBase[] miniGames) => view.InjectMiniGames(miniGames);
        
        private void OnPerformedCustomInput1(InputAction.CallbackContext callback)
        {
            view.PerformCustomInput1();
        }

        private void OnPerformedCustomInput2(InputAction.CallbackContext callback)
        {
            view.PerformCustomInput2();
        }

        private void OnLocalBoatSet(IBoat boat)
        {
            _localBoat = boat;
            view.RegisterLocalBoatEvent(boat);
        }
        
        private void OnLocalBoatRemove(IBoat boat)
        {
            _localBoat = null;
            view.UnRegisterLocalBoatEvent(boat);
        }

        private void OnFishingActionStarted(IFishingSpot fishingSpot)
        {
            view.FishingActionStarted(fishingSpot);
        } 
        
        private void OnFishingActionCanceled()
        {
            view.FishingActionCanceled();
        } 
        
        private void OnFishingActionFailed()
        {
            view.FishingActionFailed();
        }
        
        private void OnFishingActionCompleted()
        {
            _localBoat.CompleteFishing();
        }
    }
}