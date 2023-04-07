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
        private IMiniGameLogic[] _allMiniGames;
        private IMiniGameLogic _activeMiniGame;
        private IBoat _localBoat;
        
        [Header("Fishing Failed Configuration")] //TODO MOVE THIS TO MVP
        [SerializeField] private TextMeshProUGUI fishingFailedText;
        [SerializeField] private float failedFishingTextSpeed;
        [SerializeField] private float failedFishingTextTimeShowing;
        
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
                    Tuple<FishingSpotType, int> fishingSpotData = spot.GetFishingSpotData();
                    foreach (var miniGame in _allMiniGames)
                    {
                        if (miniGame.MiniGameType == fishingSpotData.Item1)
                        {
                            _activeMiniGame = miniGame;
                            _activeMiniGame.OnMiniGameComplete += CompleteMiniGame;
                            _activeMiniGame.StartMiniGame(spot);
                            return;
                        }
                    }
                };
                
                boat.OnFishingActionCanceled += delegate
                {
                    _activeMiniGame.CloseMiniGame();
                    miniGamesPanel.SetActive(false);
                    _activeMiniGame = null;
                };
                
                boat.OnFishingActionFailed += delegate //TODO MOVE THIS TO MVP
                {
                    if(_fishingActionFailedIEnumerator != null)
                        StopCoroutine(_fishingActionFailedIEnumerator);
                    _fishingActionFailedIEnumerator = FishingActionFailedCoroutine();
                    StartCoroutine(_fishingActionFailedIEnumerator);
                };
            };
            
            CustomInput.Input.MiniGames.MiniGameInput1.performed += ReceiveInput1;
            CustomInput.Input.MiniGames.MiniGameInput2.performed += ReceiveInput2;
            
            _failedFishingWaitForSeconds = new WaitForSeconds(failedFishingTextTimeShowing);
        }

        private void OnDestroy()
        {
            CustomInput.Input.MiniGames.MiniGameInput1.performed -= ReceiveInput1;
            CustomInput.Input.MiniGames.MiniGameInput2.performed -= ReceiveInput2;
        }
        
        private void ReceiveInput1(InputAction.CallbackContext context) => _activeMiniGame?.ReceiveMiniGameInput1();
        private void ReceiveInput2(InputAction.CallbackContext context) => _activeMiniGame?.ReceiveMiniGameInput2();

        private void CompleteMiniGame()
        {
            _localBoat.CompleteFishing();
            _activeMiniGame.CloseMiniGame();
            miniGamesPanel.SetActive(false);
            _activeMiniGame = null;
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

