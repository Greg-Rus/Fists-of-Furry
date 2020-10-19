using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig", order = 3)]
public class PlayerConfig : ScriptableObject
{
    public float AttackRange;
    public float WalkingSpeed;
    public float ChargingSpeed;
}