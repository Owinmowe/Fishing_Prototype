using System;
using System.Collections;
using FishingPrototype.Gameplay.Boat;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingPrototype.Gameplay.Minigames
{
    public class FishingMiniGameControl : MonoBehaviour
    {

        [SerializeField] private GameObject miniGamesPanel;
        [SerializeField] private GameObject fishingDataPanel;
        private IMiniGameLogic[] _allMiniGames;
        private IMiniGameLogic _activeMiniGame;
        
        private IBoat _localBoat;
        private IFishingSpot _lastFishingSpot;
        
        [Header("Fishing Failed Configuration")] //TODO MOVE THIS TO MVP
        [SerializeField] private TextMeshProUGUI fishingFailedText;
        [SerializeField] private float failedFishingTextSpeed;
        [SerializeField] private float failedFishingTextTimeShowing;
        
        [Header("Fishing Data Configuration")] //TODO MOVE THIS TO MVP
        [SerializeField] private TextMeshProUGUI fishTypeText;
        [SerializeField] private TextMeshProUGUI fishAmountText;
        
        private WaitForSeconds _failedFishingWaitForSeconds;
        private IEnumerator _fishingActionFailedIEnumerator;

        void Awake()
        {
            _allMiniGames = GetComponentsInChildren<IMiniGameLogic>(includeInactive: true);
            
            CustomInput.Input.MiniGamesControl.MiniGameInput1.performed += ReceiveInput1; //TODO MOVE THIS TO MVP
            CustomInput.Input.MiniGamesControl.MiniGameInput2.performed += ReceiveInput2; //TODO MOVE THIS TO MVP
            
            _failedFishingWaitForSeconds = new WaitForSeconds(failedFishingTextTimeShowing);

            IBoat.onLocalBoatSet += SetLocalBoat;
        }

        private void OnDestroy()
        {
            CustomInput.Input.MiniGamesControl.MiniGameInput1.performed -= ReceiveInput1; //TODO MOVE THIS TO MVP
            CustomInput.Input.MiniGamesControl.MiniGameInput2.performed -= ReceiveInput2; //TODO MOVE THIS TO MVP
            
            IBoat.onLocalBoatSet -= SetLocalBoat;
            if (_localBoat != null)
            {
                _localBoat.OnFishingActionStarted -= SetFishingActionStarted;
                _localBoat.OnFishingActionCanceled -= SetFishingActionCanceled;
                _localBoat.OnFishingActionFailed -= SetFishingActionFailed;
            }
        }

        private void SetLocalBoat(IBoat boat)
        {
            _localBoat = boat;

            boat.OnFishingActionStarted += SetFishingActionStarted;
            boat.OnFishingActionCanceled += SetFishingActionCanceled;
            boat.OnFishingActionFailed += SetFishingActionFailed;
        }

        private void SetFishingActionStarted(IFishingSpot spot)
        {
            miniGamesPanel.SetActive(true);
            fishingDataPanel.SetActive(true);

            _lastFishingSpot = spot;
            Tuple<FishingSpotType, int> fishingSpotData = _lastFishingSpot.GetFishingSpotData();

            fishTypeText.text = "Fish Type: " + Enum.GetName(typeof(FishingSpotType), fishingSpotData.Item1);
            fishAmountText.text = "Fish Amount: " + fishingSpotData.Item2;

            _lastFishingSpot.OnFishAmountChanged += OnFishAmountChange;

            foreach (var miniGame in _allMiniGames)
            {
                if (miniGame.MiniGameType == fishingSpotData.Item1)
                {
                    _activeMiniGame = miniGame;
                    _activeMiniGame.OnMiniGameComplete += CompleteMiniGame;
                    _activeMiniGame.StartMiniGame(_lastFishingSpot);
                    return;
                }
            }
        }

        private void SetFishingActionCanceled()
        {
            _activeMiniGame.CloseMiniGame();
            miniGamesPanel.SetActive(false);
            fishingDataPanel.SetActive(false);
            _activeMiniGame = null;

            _lastFishingSpot.OnFishAmountChanged -= OnFishAmountChange;
            _lastFishingSpot = null;
        }
        
        private void SetFishingActionFailed() //TODO MOVE THIS TO MVP
        {
            if (_fishingActionFailedIEnumerator != null)
                StopCoroutine(_fishingActionFailedIEnumerator);
            _fishingActionFailedIEnumerator = FishingActionFailedCoroutine();
            StartCoroutine(_fishingActionFailedIEnumerator);
        }
        
        private void ReceiveInput1(InputAction.CallbackContext context) => _activeMiniGame?.ReceiveMiniGameInput1(); //TODO MOVE THIS TO MVP
        private void ReceiveInput2(InputAction.CallbackContext context) => _activeMiniGame?.ReceiveMiniGameInput2(); //TODO MOVE THIS TO MVP

        private void CompleteMiniGame()
        {
            _localBoat.CompleteFishing();
            _activeMiniGame.CloseMiniGame();
            miniGamesPanel.SetActive(false);
            fishingDataPanel.SetActive(false);
            _activeMiniGame = null;
            
            _lastFishingSpot.OnFishAmountChanged -= OnFishAmountChange;
            _lastFishingSpot = null;
        }

        private void OnFishAmountChange(int fishAmount)
        {
            fishAmountText.text = "Fish Amount: " + fishAmount;
        }
        
        private IEnumerator FishingActionFailedCoroutine() //TODO MOVE THIS TO MVP
        {
            fishingFailedText.gameObject.SetActive(true);
            
            float t = 0;
            Color textColor = fishingFailedText.color;

            while (t < 1)
            {
                textColor.a = t;
                fishingFailedText.color = textColor;
                t += Time.deltaTime * failedFishingTextSpeed;
                yield return null;
            }
            
            t = 1;
            textColor.a = t;
            fishingFailedText.color = textColor;
            
            yield return _failedFishingWaitForSeconds;

            while (t > 0)
            {
                textColor.a = t;
                fishingFailedText.color = textColor;
                t -= Time.deltaTime * failedFishingTextSpeed;
                yield return null;
            }
            
            t = 0;
            textColor.a = t;
            fishingFailedText.color = textColor;
            
            fishingFailedText.gameObject.SetActive(false);
        }
    }
}

