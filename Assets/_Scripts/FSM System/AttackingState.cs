using System;
using _Scripts.FSM_System;
using UnityEngine;
using UnityEngine.AI;

public class AttackingState : FSMState
{
    private FSMSystem _fsm;
    private readonly UserInput _userInput;
    private readonly TargetSelector _targetSelector;
    private readonly PlayerConfig _playerConfig;
    private readonly NavMeshAgent _navigation;
    private readonly Transform _playerTransform;
    private readonly AnimationController _animation;

    protected AttackSide LastAttackSide = AttackSide.Left;

    public AttackingState(FSMSystem fsm, TargetSelector targetSelector, AnimationController animation,
        UserInput userInput, PlayerConfig playerConfig, NavMeshAgent navigation, Transform playerTransform)
    {
        _fsm = fsm;
        _targetSelector = targetSelector;
        _animation = animation;
        _userInput = userInput;
        _playerConfig = playerConfig;
        _navigation = navigation;
        _playerTransform = playerTransform;
    }

    public override void Reason()
    {
        //Handled by callbacks
    }

    public override void Act()
    {
        //Nothing to do
    }

    public override void DoBeforeEntering()
    {
        _targetSelector.EnemyCurrentlyInCombat.AI.OnUnderAttack();
        _animation.AnimationSpeed = _playerConfig.AttackAnimationSpeed;
        _animation.ApplyRootMotion = true;
        SnapToTargetEnemy();
        
        _userInput.LastAttackInput = AttackType.None;
    }
    
    private void SnapToTargetEnemy()
    {
        var enemyPosition = _targetSelector.EnemyCurrentlyInCombat.transform.position;
        var directionToPlayer = _playerTransform.position - enemyPosition;
        var desiredPlayerPosition = directionToPlayer.normalized;
        _navigation.Warp(enemyPosition + desiredPlayerPosition);
    }
}