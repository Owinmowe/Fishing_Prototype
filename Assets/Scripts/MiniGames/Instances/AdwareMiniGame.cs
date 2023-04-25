using FishingPrototype.Gameplay.FishingSpot;
using UnityEngine;
using UnityEngine.UI;

namespace FishingPrototype.Gameplay.Minigames
{
    public class AdwareMiniGame : MiniGameBase
    {
        [Header("Adware MiniGame Configuration")] 
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
                    CallMiniGameCompleteEvent();
            }
            else if (_centerLocalScale.x > .1f)
            {
                _centerLocalScale.x -= Time.deltaTime * reduceSpeed;
                _centerLocalScale.y -= Time.deltaTime * reduceSpeed;
                centerImage.rectTransform.localScale = _centerLocalScale;
            }
        }

        public override FishingSpotType GetMiniGameType() => FishingSpotType.Adware;

        public override void StartMiniGame(IFishingSpot fishingSpot)
        {
            gameObject.SetActive(true);
            _currentFishingSpot = fishingSpot;
            _miniGameAmount = _currentFishingSpot.GetFishingSpotData().Item2;
            InitializeCenterImage();
        }

        public override void CloseMiniGame()
        {
            gameObject.SetActive(false);
        }

        public override void PerformMiniGameInput1()
        {
            _centerLocalScale.x += growPerPress;
            _centerLocalScale.y += growPerPress;
        }

        public override void PerformMiniGameInput2()
        {
            
        }

        public override void CancelMiniGameInput1()
        {
            
        }

        public override void CancelMiniGameInput2()
        {
            
        }

        private void InitializeCenterImage()
        {
            _centerLocalScale = new Vector3(centerImageStartSize, centerImageStartSize, 1);
            centerImage.rectTransform.localScale = _centerLocalScale;
        }
    }
}
