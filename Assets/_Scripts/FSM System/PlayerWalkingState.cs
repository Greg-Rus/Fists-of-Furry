using System.Collections;
using System.Collections.Generic;
using _Scripts.FSM_System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerWalkingState : FSMState
{
    private NavMeshAgent _navigation;
    private UserInput _userInput;
    private TargetSelector _targetSelector;
    private FSMSystem _fsm;
    private PlayerConfig _playerConfig;
    private readonly AnimationController _animation;

    public PlayerWalkingState(FSMSystem fsm, NavMeshAgent navigation, UserInput userInput, TargetSelector targetSelector, PlayerConfig playerConfig, AnimationController animation)
    {
        _navigation = navigation;
        _userInput = userInput;
        _targetSelector = targetSelector;
        _playerConfig = playerConfig;
        _animation = animation;
        _fsm = fsm;
        stateID = StateID.Walking;
    }

    public override void Reason()
    {
        if (_userInput.LastAttackInput != AttackType.None && _targetSelector.SelectedTarget == null)
        {
            _userInput.LastAttackInput = AttackType.None; //Later this will be a whiff.
        }
        else if (_userInput.LastAttackInput != AttackType.None && _targetSelector.SelectedTarget != null)
        {
            _fsm.PerformTransition(Transition.ToCharging);
        }
    }

    public override void Act()
    {
        _navigation.SetDestination(_userInput.InputDestination);
    }

    public override void DoBeforeEntering()
    {
        _navigation.enabled = true;
        _navigation.speed = _playerConfig.WalkingSpeed;
        _navigation.acceleration = _playerConfig.WalkingAcceleration;
        _animation.AnimationSpeed = _playerConfig.DefaultAnimationSpeed;
        _animation.ApplyRootMotion = false;
    }
}