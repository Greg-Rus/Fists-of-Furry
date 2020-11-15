using System;
using _Scripts.FSM_System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackingState : FSMState
{
    private readonly Transform _playerTransform;
    private Transform _ownTransform;
    private readonly AnimationController _animation;
    private readonly NavMeshAgent _navigation;
    private readonly EnemyConfig _enemyConfig;

    public EnemyAttackingState(Transform playerTransform, Transform ownTransform, AnimationController animation, NavMeshAgent navigation, EnemyConfig enemyConfig)
    {
        _playerTransform = playerTransform;
        _ownTransform = ownTransform;
        _animation = animation;
        _navigation = navigation;
        _enemyConfig = enemyConfig;
        stateID = StateID.Attacking;
    }

    public override void Reason()
    {

    }

    public override void Act()
    {

    }

    public override void DoBeforeEntering()
    {
        _animation.AnimationSpeed = _enemyConfig.AttackAnimationSpeed;
        _animation.ApplyRootMotion = true;
        
        var snappedPosition = Helpers.SnapPositionToUnitFromTarget(_ownTransform.position, _playerTransform.position);
        _navigation.Warp(snappedPosition);
        
        _animation.StartPunchAnimation(1, AttackSide.Right);
    }
}