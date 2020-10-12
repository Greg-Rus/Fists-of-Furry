using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 2)]
public class EnemyConfig : ScriptableObject
{
    public float AttackRange;
    public float WalkingSpeed;
    public float RagdollExplosionForce;
    public float RagdollUpwardsModifier;
    public float DestroyTimer;
}