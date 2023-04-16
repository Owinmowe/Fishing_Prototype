using System.Collections.Generic;
using FishingPrototype.Gameplay.Input;
using FishingPrototype.Waves;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingPrototype.Test
{
    public class WaterMaterialChanger : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private List<MaterialData> materialsData;
        [SerializeField] private TextMeshProUGUI materialNameComponent;
        [SerializeField] private TextMeshProUGUI materialDescriptionComponent;
        private int materialIndex = 0;

        private void Start()
        {
            SetNewMaterial();
            CustomInput.Input.TestControl.TestButton1.performed += ChangeMaterialIndex;
        }

        private void OnDestroy()
        {
            CustomInput.Input.TestControl.TestButton1.performed -= ChangeMaterialIndex;
        }

        private void ChangeMaterialIndex(InputAction.CallbackContext callback)
        {
            materialIndex++;
            if (materialIndex == materialsData.Count)
                materialIndex = 0;

            SetNewMaterial();
        }

        private void SetNewMaterial()
        {
            meshRenderer.material = materialsData[materialIndex].material;
            materialNameComponent.text = materialsData[materialIndex].material.name;
            materialDescriptionComponent.text = materialsData[materialIndex].materialDescription;
            WaveManager.Get().UpdateMaterialReference();
        }
        
        [System.Serializable]
        public struct MaterialData
        {
            public Material material;
            [SerializeField] public string materialDescription;
        }
    }
}
