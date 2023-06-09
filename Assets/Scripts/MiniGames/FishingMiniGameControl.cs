using System;
using System.Collections;
using FishingPrototype.Gameplay.FishingSpot;
using FishingPrototype.Gameplay.FishingSpot.Data;
using TMPro;
using UnityEngine;

namespace FishingPrototype.Gameplay.Minigames
{
    public class FishingMiniGameControl : MonoBehaviour
    {

        public Action OnFishingCompleted;
        
        [SerializeField] private GameObject miniGamesPanel;
        [SerializeField] private GameObject fishingDataPanel;
        private MiniGameBase[] _allMiniGames;
        private MiniGameBase _activeMiniGame;
        
        private IFishingSpot _lastFishingSpot;
        
        [Header("Fishing Failed Configuration")]
        [SerializeField] private TextMeshProUGUI fishingFailedText;
        [SerializeField] private float failedFishingTextSpeed;
        [SerializeField] private float failedFishingTextTimeShowing;
        
        [Header("Fishing Data Configuration")]
        [SerializeField] private TextMeshProUGUI fishTypeText;
        [SerializeField] private TextMeshProUGUI fishAmountText;
        
        private WaitForSeconds _failedFishingWaitForSeconds;
        private IEnumerator _fishingActionFailedIEnumerator;

        void Awake()
        {
            _failedFishingWaitForSeconds = new WaitForSeconds(failedFishingTextTimeShowing);
            miniGamesPanel.gameObject.SetActive(false);
        }
        
        public void SetAllMiniGames(MiniGameBase[] miniGamesPrefabs)
        {
            _allMiniGames = new MiniGameBase[miniGamesPrefabs.Length];
            for (int i = 0; i < miniGamesPrefabs.Length; i++)
            {
                if (miniGamesPrefabs[i] != null)
                {
                    _allMiniGames[i] = Instantiate(miniGamesPrefabs[i], miniGamesPanel.transform);
                    _allMiniGames[i].gameObject.SetActive(false);
                }
            }
        }

        public void StartFishing(IFishingSpot spot)
        {
            miniGamesPanel.SetActive(true);
            fishingDataPanel.SetActive(true);

            _lastFishingSpot = spot;
            FishingSpotData fishingSpotData = _lastFishingSpot.GetFishingSpotData();

            fishTypeText.text = "Fish Type: " + Enum.GetName(typeof(FishingSpotType), fishingSpotData.type);
            fishAmountText.text = "Fish Amount: " + fishingSpotData.amount;

            _lastFishingSpot.OnFishAmountChanged += OnFishAmountChange;

            foreach (var miniGame in _allMiniGames)
            {
                if (miniGame.GetMiniGameType() == fishingSpotData.type)
                {
                    _activeMiniGame = miniGame;
                    _activeMiniGame.OnMiniGameComplete += CompleteMiniGame;
                    _activeMiniGame.StartMiniGame(_lastFishingSpot);
                    return;
                }
            }
        }

        public void CancelFishing()
        {
            _activeMiniGame.CloseMiniGame();
            miniGamesPanel.SetActive(false);
            fishingDataPanel.SetActive(false);
            _activeMiniGame = null;

            _lastFishingSpot.OnFishAmountChanged -= OnFishAmountChange;
            _lastFishingSpot = null;
        }
        
        public void FailFishing()
        {
            if (_fishingActionFailedIEnumerator != null)
                StopCoroutine(_fishingActionFailedIEnumerator);
            _fishingActionFailedIEnumerator = FishingActionFailedCoroutine();
            StartCoroutine(_fishingActionFailedIEnumerator);
        }
        
        public void PerformInput1()
        {
            if(_activeMiniGame != null)
                _activeMiniGame.PerformMiniGameInput1();
        }

        public void PerformInput2()
        {
            if(_activeMiniGame != null)
                _activeMiniGame.PerformMiniGameInput2();
        }

        public void CancelInput1()
        {
            if(_activeMiniGame != null)
                _activeMiniGame.CancelMiniGameInput1();
        }

        public void CancelInput2()
        {
            if(_activeMiniGame != null)
                _activeMiniGame.CancelMiniGameInput2();
        }
        
        private void CompleteMiniGame()
        {
            _activeMiniGame.CloseMiniGame();
            miniGamesPanel.SetActive(false);
            fishingDataPanel.SetActive(false);
            _activeMiniGame = null;
            
            _lastFishingSpot.OnFishAmountChanged -= OnFishAmountChange;
            _lastFishingSpot = null;
            OnFishingCompleted?.Invoke();
        }

        private void OnFishAmountChange(int fishAmount)
        {
            fishAmountText.text = "Fish Amount: " + fishAmount;
        }
        
        private IEnumerator FishingActionFailedCoroutine()
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

