﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 2)]
public class EnemyConfig : ScriptableObject
{
    public float AttackRange;
    public float WalkingSpeed;
    public float RagdollExplosionForce;
    public float RagdollUpwardsModifier;
    public float DestroyTimer;
    public HpProbabilityMapping[] HpProbabilityMappings;
    public float AttackAnimationSpeed;
    public float AttackWindDownAnimationSpeed;
    public float DefaultAnimationSpeed;
}

[Serializable]
public struct HpProbabilityMapping
{
    public int Hp;
    public float Range;
}