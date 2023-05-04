using System;
using UnityEditor;
using UnityEngine;

namespace FishingPrototype.Gameplay.Maps.Data
{
    [CustomPropertyDrawer(typeof(SpawnChances))]
    public class SpawnChancesDrawer : PropertyDrawer
    {
        private const int CHANCES_LABEL_SIZE = 15;
        private const float PROPERTY_BASE_SIZE = 40f;
        private const float SEPARATION_BASE_SIZE = 10f;
        
        private string[] _fishingSpotTypeNames;
        
        private const int PROPERTIES_AMOUNT = 3;
        private SerializedProperty _chanceListProperty;
        private SerializedProperty _minListProperty;
        private SerializedProperty _maxListProperty;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float size = PROPERTY_BASE_SIZE;
            
            _fishingSpotTypeNames ??= Enum.GetNames(typeof(FishingSpotType));
            _chanceListProperty ??= property.FindPropertyRelative(nameof(SpawnChances.chanceTypeList));
            _minListProperty ??= property.FindPropertyRelative(nameof(SpawnChances.chanceAmountMinList));
            _maxListProperty ??= property.FindPropertyRelative(nameof(SpawnChances.chanceAmountMaxList));

            if (_chanceListProperty.arraySize != _fishingSpotTypeNames.Length)
            {
                _chanceListProperty.ClearArray();
                for (int i = 0; i < _fishingSpotTypeNames.Length; i++)
                {
                    _chanceListProperty.InsertArrayElementAtIndex(i);
                }
            }

            if (_minListProperty.arraySize != _fishingSpotTypeNames.Length)
            {
                _minListProperty.ClearArray();
                for (int i = 0; i < _fishingSpotTypeNames.Length; i++)
                {
                    _minListProperty.InsertArrayElementAtIndex(i);
                }
            }
            
            if (_maxListProperty.arraySize != _fishingSpotTypeNames.Length)
            {
                _maxListProperty.ClearArray();
                for (int i = 0; i < _fishingSpotTypeNames.Length; i++)
                {
                    _maxListProperty.InsertArrayElementAtIndex(i);
                }
            }
            
            for (int i = 0; i < _fishingSpotTypeNames.Length; i++)
            {
                GUIContent enumLabel = new GUIContent(_fishingSpotTypeNames[i]);
                size += EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, enumLabel) * i * PROPERTIES_AMOUNT;
            }
            
            return size;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            GUIContent chancesLabel = new GUIContent("Spawn Configuration");
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = CHANCES_LABEL_SIZE,
            };
            titleStyle.normal.textColor = Color.red;

            float labelPositionX = position.x;
            float labelPositionY = PROPERTY_BASE_SIZE + titleStyle.CalcSize(chancesLabel).y;
            float labelWidth = position.width;
            float labelHeight = titleStyle.CalcSize(chancesLabel).y;

            Rect rectLabel = new Rect(labelPositionX, labelPositionY, labelWidth, labelHeight);
            EditorGUI.LabelField(rectLabel, chancesLabel, titleStyle);
            
            GUIContent enumLabel = new GUIContent(_fishingSpotTypeNames[0]);
            float rectPositionX = position.x + SEPARATION_BASE_SIZE / 2;
            float rectPositionY = EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, enumLabel) + PROPERTY_BASE_SIZE;
            float rectWidth = position.width - SEPARATION_BASE_SIZE / 2;
            float rectHeight = EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, enumLabel);
            Rect rectProperties = new Rect(rectPositionX, rectPositionY, rectWidth, rectHeight);

            for (int i = 0; i < _fishingSpotTypeNames.Length; i++)
            {
                rectProperties.y += EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, enumLabel) + SEPARATION_BASE_SIZE;

                Rect boxRect = rectProperties;
                boxRect.height *= PROPERTIES_AMOUNT + 1;
                boxRect.height += SEPARATION_BASE_SIZE / 2;
                boxRect.width += SEPARATION_BASE_SIZE;
                boxRect.position =  new Vector2(boxRect.x - SEPARATION_BASE_SIZE / 2, boxRect.y);
                GUI.Box(boxRect, GUIContent.none, GUI.skin.window);
                
                enumLabel = new GUIContent(_fishingSpotTypeNames[i]);
                EditorGUI.LabelField(rectProperties, enumLabel, titleStyle);
                
                rectProperties.y += EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, enumLabel);
                
                GUIContent chanceLabel = new GUIContent("Chance Percentage");
                GUIContent minAmountLabel = new GUIContent("Min Spawn Amount");
                GUIContent maxAmountLabel = new GUIContent("Max Spawn Amount");
                
                float chanceValue = _chanceListProperty.GetArrayElementAtIndex(i).floatValue;
                int minAmountValue = _minListProperty.GetArrayElementAtIndex(i).intValue;
                int maxAmountValue = _maxListProperty.GetArrayElementAtIndex(i).intValue;
                
                _chanceListProperty.GetArrayElementAtIndex(i).floatValue = EditorGUI.Slider(rectProperties, chanceLabel, chanceValue, 0, 100);
                rectProperties.y += EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, enumLabel);
                _minListProperty.GetArrayElementAtIndex(i).intValue = EditorGUI.IntSlider(rectProperties, minAmountLabel, minAmountValue, 1, maxAmountValue);
                rectProperties.y += EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, enumLabel);
                _maxListProperty.GetArrayElementAtIndex(i).intValue = EditorGUI.IntSlider(rectProperties, maxAmountLabel, maxAmountValue,  minAmountValue, 100);
            }
            
            EditorGUI.EndProperty();
        }
    }
}
