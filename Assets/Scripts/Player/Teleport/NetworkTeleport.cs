using System.Collections;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace FishingPrototype.Gameplay.Boat
{
    public class NetworkTeleport : NetworkBehaviour, ITeleportComponent
    {
        [SerializeField] private MeshRenderer boatRenderer;
        
        private const string DISSOLVE_PROPERTY_NAME = "_Dissolve";
        private int _dissolvePropertyId;
        private IEnumerator _teleportIEnumerator;
        private const float MIN_DISSOLVE_VALUE = -.1f;
        private const float MAX_DISSOLVE_VALUE = 1;

        public void Awake()
        {
            _dissolvePropertyId = Shader.PropertyToID(DISSOLVE_PROPERTY_NAME);
        }

        public void Teleport(Vector3 newPosition)
        {
            HostTeleport(newPosition);
            RpcTeleport();
        }

        public async Task Dissolve()
        {
            RpcDissolve();
            float t = MIN_DISSOLVE_VALUE;
            while (t < MAX_DISSOLVE_VALUE)
            {
                t += Time.deltaTime;
                boatRenderer.material.SetFloat(_dissolvePropertyId, t);
                await Task.Yield();
            }
            boatRenderer.material.SetFloat(_dissolvePropertyId, MAX_DISSOLVE_VALUE);
        }

        public async Task UnDissolve()
        {
            RpcUnDissolve();
            float t = MAX_DISSOLVE_VALUE;
            while (t > MIN_DISSOLVE_VALUE)
            {
                t -= Time.deltaTime;
                boatRenderer.material.SetFloat(_dissolvePropertyId, t);
                await Task.Yield();
            }
            boatRenderer.material.SetFloat(_dissolvePropertyId, MIN_DISSOLVE_VALUE);
        }

        [ClientRpc]
        private async void RpcDissolve()
        {
            await Dissolve();
        }
        

        [ClientRpc]
        private async void RpcUnDissolve()
        { 
            await UnDissolve();
        }
        
        private async void HostTeleport(Vector3 newPosition)
        {
            await Dissolve();
            transform.position = newPosition;
            await UnDissolve();
        }
        
        [ClientRpc]
        private async void RpcTeleport()
        {
            await Dissolve();
            await UnDissolve();
        }
    }
}