using System;
using System.Collections;
using FishingPrototype.Gameplay.Boat;
using FishingPrototype.Gameplay.FishingSpot;
using TMPro;
using UnityEngine;

namespace FishingPrototype.Test
{
    public class TestFishIterationUI : MonoBehaviour
    {

        [Header("Fishing Panel")] 
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI fishTypeText;
        [SerializeField] private TextMeshProUGUI fishAmountText;
        
        [Header("Failed Fishing")]
        [SerializeField] private TextMeshProUGUI failedActionText;
        [SerializeField] private float failedFishingTextSpeed;
        [SerializeField] private float failedFishingTextTimeShowing;

        private WaitForSeconds _failedFishingWaitForSeconds;
        private IEnumerator _fishingActionFailedIEnumerator;
        
        private void Awake()
        {
            IBoat.onLocalBoatSet += OnLocalFishingBoatSet;
            _failedFishingWaitForSeconds = new WaitForSeconds(failedFishingTextTimeShowing);
        }

        private void OnLocalFishingBoatSet(IBoat boat)
        {
            boat.OnFishingActionStarted += delegate(IFishingSpot fishingSpot)
            {
                panel.SetActive(true);
                Tuple<FishingSpotType, int> data = fishingSpot.GetFishingSpotData();
                fishTypeText.text = "Fishing Type: " + Enum.GetName(typeof(FishingSpotType), data.Item1);
                fishAmountText.text = "Amount: " + data.Item2;
            }; 
            
            boat.OnFishingActionFailed += delegate
            {
                if (_fishingActionFailedIEnumerator != null)
                    StopCoroutine(_fishingActionFailedIEnumerator);

                _fishingActionFailedIEnumerator = FishingActionFailedCoroutine();
                StartCoroutine(_fishingActionFailedIEnumerator);
            };
            
            boat.OnFishingActionCanceled += delegate
            {
                panel.SetActive(false);
            };
        }

        private IEnumerator FishingActionFailedCoroutine()
        {
            failedActionText.gameObject.SetActive(true);
            
            float t = 0;
            Color textColor = failedActionText.color;

            while (t < 1)
            {
                textColor.a = t;
                failedActionText.color = textColor;
                t += Time.deltaTime * failedFishingTextSpeed;
                yield return null;
            }
            
            t = 1;
            textColor.a = t;
            failedActionText.color = textColor;
            
            yield return _failedFishingWaitForSeconds;

            while (t > 0)
            {
                textColor.a = t;
                failedActionText.color = textColor;
                t -= Time.deltaTime * failedFishingTextSpeed;
                yield return null;
            }
            
            t = 0;
            textColor.a = t;
            failedActionText.color = textColor;
            
            failedActionText.gameObject.SetActive(false);
        }
        
        
        private void OnDestroy()
        {
            IBoat.onLocalBoatSet += OnLocalFishingBoatSet;
        }
    }
}
 
