#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace FishingPrototype.Gameplay.Minigames.Data
{
    [CustomEditor(typeof(MiniGamesData))]
    public class MiniGamesDataEditor : Editor
    {
        private MiniGamesData _script;
        private string[] _fishingSpotTypeNames;
        private SerializedProperty _miniGamesPrefabsSerializedProperty;
        
        private void OnEnable()
        {
            _script = (MiniGamesData)target;
            _fishingSpotTypeNames = Enum.GetNames(typeof(FishingSpotType));
            _miniGamesPrefabsSerializedProperty =
                serializedObject.FindProperty(nameof(_script.miniGamesPrefabs));
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_script.miniGamesPrefabs.Length < _fishingSpotTypeNames.Length)
            {
                _script.miniGamesPrefabs = new MiniGameBase[Enum.GetValues(typeof(FishingSpotType)).Length];
            }
            
            for (int i = 0; i < _fishingSpotTypeNames.Length; i++)
            {
                EditorGUILayout.BeginVertical("Box");
                GUIContent guiContent = new GUIContent(_fishingSpotTypeNames[i]);
                EditorGUILayout.PropertyField(_miniGamesPrefabsSerializedProperty.GetArrayElementAtIndex(i), guiContent);
                EditorGUILayout.EndVertical();
 
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif