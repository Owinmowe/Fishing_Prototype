using FishingPrototype.Gameplay.FishingSpot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FishingPrototype.Gameplay.Minigames
{
    public class KeyLoggerMiniGame : MiniGameBase
    {
        [Header("Keylogger MiniGame Configuration")] 
        [SerializeField] private float growPerPress = .1f;
        [SerializeField] private float reduceSpeed = 1f;
        [SerializeField] private float centerImageStartSize = .1f;
        [SerializeField] private Image centerImage;
        [SerializeField] private TextMeshProUGUI keyPressText;
        
        private Vector3 _centerLocalScale;
        private IFishingSpot _currentFishingSpot;
        private int _miniGameAmount;
        private bool input1Correct = true;

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

        public override FishingSpotType GetMiniGameType() => FishingSpotType.KeyLogger;

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

        public override void ReceiveMiniGameInput1()
        {
            if (input1Correct)
            {
                keyPressText.text = "Press G Key";
                input1Correct = false;
                _centerLocalScale.x += growPerPress;
                _centerLocalScale.y += growPerPress;
            }
            else
            {
                _centerLocalScale.x -= growPerPress;
                _centerLocalScale.y -= growPerPress;
                if (_centerLocalScale.x < .1f)
                {
                    InitializeCenterImage();
                }
            }
        }

        public override void ReceiveMiniGameInput2()
        {
            if (!input1Correct)
            {
                keyPressText.text = "Press F Key";
                input1Correct = true;
                _centerLocalScale.x += growPerPress;
                _centerLocalScale.y += growPerPress;
            }
            else
            {
                _centerLocalScale.x -= growPerPress;
                _centerLocalScale.y -= growPerPress;
                if (_centerLocalScale.x < .1f)
                {
                    InitializeCenterImage();
                }
            }
        }
        
        private void InitializeCenterImage()
        {
            _centerLocalScale = new Vector3(centerImageStartSize, centerImageStartSize, 1);
            centerImage.rectTransform.localScale = _centerLocalScale;
        }
    }
}
