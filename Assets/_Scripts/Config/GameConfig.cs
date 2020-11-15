using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 2)]
public class GameConfig : ScriptableObject
{
    public LayerMask GroundLayers;
    public LayerMask EnemyLayers;
    public float EnemySpawnSpeed;
    public float EnemySpawnRadius;
    public float MinimumMousePositionDelta;
}