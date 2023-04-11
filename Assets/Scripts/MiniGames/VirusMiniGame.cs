using System;
using FishingPrototype.Gameplay.FishingSpot;
using UnityEngine;
using UnityEngine.UI;

namespace FishingPrototype.Gameplay.Minigames
{
    public class VirusMiniGame : MonoBehaviour, IMiniGameLogic
    {
        public event Action OnMiniGameComplete;
        public FishingSpotType MiniGameType => miniGameType;

        [SerializeField] private FishingSpotType miniGameType;

        [Header("Virus MiniGame Configuration")] 
        [SerializeField] private float growPerPress = .1f;
        [SerializeField] private float reduceSpeed = 1f;
        [SerializeField] private float centerImageStartSize = .1f;
        [SerializeField] private Image centerImage;
        
        private Vector3 _centerLocalScale;
        private IFishingSpot _currentFishingSpot;
        private int _miniGameAmount;

        private void Update()
        {
            if (_centerLocalScale.x > 1f)
            {
                InitializeCenterImage();
                _currentFishingSpot.OnCompletedFishing();
                _miniGameAmount--;
                
                if(_miniGameAmount == 0)
                    OnMiniGameComplete?.Invoke();
            }
            else if (_centerLocalScale.x > .1f)
            {
                _centerLocalScale.x -= Time.deltaTime * reduceSpeed;
                _centerLocalScale.y -= Time.deltaTime * reduceSpeed;
                centerImage.rectTransform.localScale = _centerLocalScale;
            }
        }
        
        public void StartMiniGame(IFishingSpot fishingSpot)
        {
            gameObject.SetActive(true);
            _currentFishingSpot = fishingSpot;
            _miniGameAmount = _currentFishingSpot.GetFishingSpotData().Item2;
            InitializeCenterImage();
        }

        public void CloseMiniGame()
        {
            gameObject.SetActive(false);
        }

        public void ReceiveMiniGameInput1()
        {
            _centerLocalScale.x += growPerPress;
            _centerLocalScale.y += growPerPress;
        }

        public void ReceiveMiniGameInput2()
        {
            
        }
        
        private void InitializeCenterImage()
        {
            _centerLocalScale = new Vector3(centerImageStartSize, centerImageStartSize, 1);
            centerImage.rectTransform.localScale = _centerLocalScale;
        }
    }
}
