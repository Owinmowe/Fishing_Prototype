using System;
using System.Threading.Tasks;
using UnityEngine;

namespace FishingPrototype.Gameplay.Boat
{
    public interface ITeleportComponent
    {
        void Teleport(Vector3 newPosition);
        Task Dissolve();
        Task UnDissolve();
    }
}

