using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 2)]
public class GameConfig : ScriptableObject
{
    public float EnemySpawnSpeed;
    public float EnemySpawnRadius;
}