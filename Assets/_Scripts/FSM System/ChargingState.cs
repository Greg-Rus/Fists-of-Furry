using _Scripts.FSM_System;
using UniRx;
using UnityEngine.AI;

public class ChargingState : FSMState
{
    private FSMSystem _fsm;
    private readonly NavMeshAgent _navigation;
    private readonly PlayerController _controller;
    private readonly PlayerConfig _playerConfig;
    private readonly AnimationController _animation;
    private TargetSelector _targetSelector;
    public ChargingState(FSMSystem fsm, NavMeshAgent navigation, PlayerController controller, PlayerConfig playerConfig, AnimationController animation, TargetSelector targetSelector)
    {
        _fsm = fsm;
        _navigation = navigation;
        _controller = controller;
        _playerConfig = playerConfig;
        _animation = animation;
        _targetSelector = targetSelector;
        stateID = StateID.Charging;
    }

    public override void Reason()
    {
        if (_navigation.remainingDistance <= 1.1f)
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
        _animation.IsCharging = true;
        _animation.ApplyRootMotion = false;
        _targetSelector.EnemyCurrentlyInCombat = _targetSelector.SelectedTarget;
    }

    public override void DoBeforeLeaving()
    {
        _animation.IsCharging = false;
        _navigation.speed = _playerConfig.WalkingSpeed;
    }
}