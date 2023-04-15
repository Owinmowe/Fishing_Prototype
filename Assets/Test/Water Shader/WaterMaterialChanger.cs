using System;
using System.Collections.Generic;
using FishingPrototype.Gameplay.Input;
using FishingPrototype.Waves;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingPrototype.Test
{
    public class WaterMaterialChanger : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private List<Material> materials;
        private int materialIndex = 0;

        private void Awake()
        {
            meshRenderer.material = materials[materialIndex];
        }

        private void Start()
        {
            WaveManager.Get().UpdateMaterialReference();
            CustomInput.Input.TestControl.TestButton1.performed += ChangeMaterial;
        }

        private void OnDestroy()
        {
            CustomInput.Input.TestControl.TestButton1.performed -= ChangeMaterial;
        }

        private void ChangeMaterial(InputAction.CallbackContext callback)
        {
            materialIndex++;
            if (materialIndex == materials.Count)
                materialIndex = 0;
            
            meshRenderer.material = materials[materialIndex];
            WaveManager.Get().UpdateMaterialReference();
        }
    }
}
