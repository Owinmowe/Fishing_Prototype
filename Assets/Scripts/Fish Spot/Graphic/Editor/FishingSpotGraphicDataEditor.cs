#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace FishingPrototype.Gameplay.FishingSpot
{
    [CustomEditor(typeof(FishingSpotGraphicData))]
    public class FishingSpotGraphicDataEditor : Editor
    {

        private FishingSpotGraphicData _script;
        private string[] _fishingSpotTypeNames;
        private SerializedProperty _graphicPrefabsSerializedProperty;
        
        private void OnEnable()
        {
            _script = (FishingSpotGraphicData)target;
            _fishingSpotTypeNames = Enum.GetNames(typeof(FishingSpotType));
            _graphicPrefabsSerializedProperty =
                serializedObject.FindProperty(nameof(_script.fishingSpotGraphicPrefabs));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_script.fishingSpotGraphicPrefabs.Length < _fishingSpotTypeNames.Length)
            {
                _script.fishingSpotGraphicPrefabs = new ParticleSystem[Enum.GetValues(typeof(FishingSpotType)).Length];
            }
            
            for (int i = 0; i < _fishingSpotTypeNames.Length; i++)
            {
                EditorGUILayout.BeginVertical("Box");
                GUIContent guiContent = new GUIContent(_fishingSpotTypeNames[i]);
                EditorGUILayout.PropertyField(_graphicPrefabsSerializedProperty.GetArrayElementAtIndex(i), guiContent);
                EditorGUILayout.EndVertical();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif