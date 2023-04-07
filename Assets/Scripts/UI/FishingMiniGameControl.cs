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
            
            IBoat.onLocalBoatSet += delegate(IBoat boat)
            {
                _localBoat = boat;
                
                boat.OnFishingActionStarted += delegate(IFishingSpot spot)
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
                };
                
                boat.OnFishingActionCanceled += delegate
                {
                    _activeMiniGame.CloseMiniGame();
                    miniGamesPanel.SetActive(false);
                    fishingDataPanel.SetActive(false);
                    _activeMiniGame = null;
                    
                    _lastFishingSpot.OnFishAmountChanged -= OnFishAmountChange;
                    _lastFishingSpot = null;
                };
                
                boat.OnFishingActionFailed += delegate //TODO MOVE THIS TO MVP
                {
                    if(_fishingActionFailedIEnumerator != null)
                        StopCoroutine(_fishingActionFailedIEnumerator);
                    _fishingActionFailedIEnumerator = FishingActionFailedCoroutine();
                    StartCoroutine(_fishingActionFailedIEnumerator);
                };
            };
            
            CustomInput.Input.MiniGamesControl.MiniGameInput1.performed += ReceiveInput1;
            CustomInput.Input.MiniGamesControl.MiniGameInput2.performed += ReceiveInput2;
            
            _failedFishingWaitForSeconds = new WaitForSeconds(failedFishingTextTimeShowing);
        }

        private void OnDestroy()
        {
            CustomInput.Input.MiniGamesControl.MiniGameInput1.performed -= ReceiveInput1;
            CustomInput.Input.MiniGamesControl.MiniGameInput2.performed -= ReceiveInput2;
        }
        
        private void ReceiveInput1(InputAction.CallbackContext context) => _activeMiniGame?.ReceiveMiniGameInput1();
        private void ReceiveInput2(InputAction.CallbackContext context) => _activeMiniGame?.ReceiveMiniGameInput2();

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

