using System.Collections.Generic;
using UnityEngine;

namespace FishingPrototype.Utils
{
    public class MaterialPropertiesHelper
    {
        private readonly Dictionary<string, int> _propertiesDictionary = new ();

        public void AddProperty(string propertyName)
        {
            _propertiesDictionary.Add(propertyName, Shader.PropertyToID(propertyName));
        }

        public int GetPropertyId(string propertyName) => _propertiesDictionary[propertyName];

        public bool MaterialHasAllProperties(Material material)
        {
            int propertiesAmount = 0;
            foreach (KeyValuePair<string, int> propertyPair in _propertiesDictionary)
            {
                if (material.HasProperty(propertyPair.Value)) propertiesAmount++;
            }
            return propertiesAmount == _propertiesDictionary.Count;
        }
    }
}

