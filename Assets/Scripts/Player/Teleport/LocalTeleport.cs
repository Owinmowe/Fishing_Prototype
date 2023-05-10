using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace FishingPrototype.Gameplay.Boat
{
    public class LocalTeleport : MonoBehaviour, ITeleportComponent
    {
        [SerializeField] private MeshRenderer boatRenderer;
        
        public Action OnDissolve { get; }
        public Action OnUnDissolve { get; }
        
        private const string DISSOLVE_PROPERTY_NAME = "_Dissolve";
        private int _dissolvePropertyId;
        private IEnumerator _teleportIEnumerator;

        private void Awake()
        {
            _dissolvePropertyId = Shader.PropertyToID(DISSOLVE_PROPERTY_NAME);
        }

        public async void Teleport(Vector3 newPosition)
        {
            await Dissolve();
            transform.position = newPosition;
            await UnDissolve();
        }

        public async Task Dissolve()
        {
            float t = -1;
            while (t < 1)
            {
                t += Time.deltaTime;
                boatRenderer.material.SetFloat(_dissolvePropertyId, t);
                await Task.Yield();
            }
            boatRenderer.material.SetFloat(_dissolvePropertyId, 1);
            OnDissolve?.Invoke();
        }

        public async Task UnDissolve()
        {
            float t = 1;
            while (t > -1)
            {
                t -= Time.deltaTime;
                boatRenderer.material.SetFloat(_dissolvePropertyId, t);
                await Task.Yield();
            }
            boatRenderer.material.SetFloat(_dissolvePropertyId, -1);
            OnUnDissolve?.Invoke();
        }
    }
}