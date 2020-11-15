using _Scripts.FSM_System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class PlayerChargingState : FSMState
{
    private FSMSystem _fsm;
    private readonly NavMeshAgent _navigation;
    private readonly PlayerController _controller;
    private readonly PlayerConfig _playerConfig;
    private readonly AnimationController _animation;
    private TargetSelector _targetSelector;
    private readonly Transform _playerTransform;
    private bool _pathSet;

    public PlayerChargingState(FSMSystem fsm, NavMeshAgent navigation, PlayerController controller, PlayerConfig playerConfig, 
        AnimationController animation, TargetSelector targetSelector, Transform playerTransform)
    {
        _fsm = fsm;
        _navigation = navigation;
        _controller = controller;
        _playerConfig = playerConfig;
        _animation = animation;
        _targetSelector = targetSelector;
        _playerTransform = playerTransform;
        stateID = StateID.Charging;
    }

    public override void Reason()
    {
        if ((_targetSelector.EnemyCurrentlyInCombat.transform.position - _playerTransform.position).sqrMagnitude <= 0.2f)
        {
            _fsm.PerformTransition(Transition.ToTiedInCombat);
        }
    }

    public override void Act()
    {
        _navigation.SetDestination(_targetSelector.EnemyCurrentlyInCombat.transform.position);
    }

    public override void DoBeforeEntering()
    {
        _navigation.enabled = true;
        _navigation.speed = _playerConfig.ChargingSpeed;
        _navigation.acceleration = _playerConfig.ChargingAcceleration;
        _animation.IsCharging = true;
        _animation.ApplyRootMotion = false;
        _targetSelector.EnemyCurrentlyInCombat = _targetSelector.SelectedTarget;
        _targetSelector.EnemyCurrentlyInCombat.TieInCombat();
        
        if(_playerConfig.InstantChargeCompletion) _fsm.PerformTransition(Transition.ToTiedInCombat);
    }

    public override void DoBeforeLeaving()
    {
        _animation.IsCharging = false;
        _navigation.ResetPath();
        _navigation.velocity = Vector3.zero;
    }
}