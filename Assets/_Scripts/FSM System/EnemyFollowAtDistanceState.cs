using _Scripts.FSM_System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowAtDistanceState : FSMState
{
    private readonly FSMSystem _fsm;
    private readonly Transform _target;
    private readonly Transform _self;
    private readonly EnemyConfig _config;
    private readonly NavMeshAgent _navigation;

    public EnemyFollowAtDistanceState(FSMSystem fsm, Transform target, Transform self, EnemyConfig config, NavMeshAgent navigation)
    {
        _fsm = fsm;
        _target = target;
        _self = self;
        _config = config;
        _navigation = navigation;
        stateID = StateID.FollowingAtDistance;
    }

    public override void Reason()
    {
        //Exit transition handled externally 
    }

    public override void Act()
    {
        _navigation.SetDestination(DesiredPosition);
        _self.LookAt(_target, Vector3.up);
    }
    
    private Vector3 DesiredPosition
    {
        get
        {
            var enemyPosition = _self.position;
            var targetPosition = _target.position;
            var directionFromTarget = enemyPosition - targetPosition;
            return (directionFromTarget.normalized * _config.AttackRange) + targetPosition;
        }
    }
}