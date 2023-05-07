using System.Collections.Generic;
using UnityEngine;
using FishingPrototype.Gameplay.Maps;
using FishingPrototype.Gameplay.Maps.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MapObject))]
public class MapObjectGizmos : MonoBehaviour
{
    
    [Header("Player Spawn")]
    [SerializeField] private Color playersGizmosColor = Color.blue;
    [SerializeField] private int playersGizmosFontSize = 12;
    [SerializeField] private Vector3 playersTextOffset = Vector3.zero;
    [SerializeField] private float playersGizmosDiskRadius = 10f;
    
    [Header("Boss Spawn")]
    [SerializeField] private Color bossGizmosColor = Color.black;
    [SerializeField] private int bossGizmosFontSize = 12;
    [SerializeField] private Vector3 bossTextOffset = Vector3.zero;
    [SerializeField] private float bossGizmosDiskRadius = 10f;
    
    [Header("Easy Spawn")]
    [SerializeField] private Color easyGizmosColor = Color.green;
    
    [Header("Medium Spawn")]
    [SerializeField] private Color mediumGizmosColor = Color.yellow;
    
    [Header("Hard Spawn")]
    [SerializeField] private Color hardGizmosColor = Color.red;
    
    private MapObject _mapObject;


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if(_mapObject == null) _mapObject = GetComponent<MapObject>();
        
        DrawPlayerSpawnPositions();
        DrawBossesSpawnPositions();
        DrawGenericSpawnPositions(_mapObject.EasyFishSpawnPositions, easyGizmosColor);
        DrawGenericSpawnPositions(_mapObject.MediumFishSpawnPositions, mediumGizmosColor);
        DrawGenericSpawnPositions(_mapObject.HardFishSpawnPositions, hardGizmosColor);
    }

    private void DrawPlayerSpawnPositions()
    {
        foreach (var playerSpawnPosition in _mapObject.PlayersSpawnPosition)
        {
            if(playerSpawnPosition == null) continue;
            Handles.color = playersGizmosColor;
            GUIStyle style = new GUIStyle
            {
                normal =
                {
                    textColor = playersGizmosColor
                },
                fontSize = playersGizmosFontSize
            };
            var position = playerSpawnPosition.position;
            Handles.Label(position + playersTextOffset, "Player Spawn Position", style);
            Handles.DrawWireDisc(position, Vector3.up, playersGizmosDiskRadius);
        }
    }
    
    private void DrawBossesSpawnPositions()
    {
        foreach (var bossSpawnPosition in _mapObject.BossSpawnPositions)
        {
            if(bossSpawnPosition == null) continue;
                
            Handles.color = bossGizmosColor;
            GUIStyle style = new GUIStyle
            {
                normal =
                {
                    textColor = bossGizmosColor
                },
                fontSize = bossGizmosFontSize
            };
            var position = bossSpawnPosition.position;
            Handles.Label(position + bossTextOffset, "Boss Spawn Position", style);
            Handles.DrawWireDisc(position, Vector3.up, bossGizmosDiskRadius);
        }
    }

    private void DrawGenericSpawnPositions(List<SpawnData> spawnsData, Color color)
    {
        foreach (var spawnData in spawnsData)
        {
            if(spawnData.spawnPosition == null) continue;
            Handles.color = color;
            Handles.DrawWireDisc(spawnData.spawnPosition.position, Vector3.up, spawnData.spawnVarianceDistance);
        }
    }
    
#endif
}
