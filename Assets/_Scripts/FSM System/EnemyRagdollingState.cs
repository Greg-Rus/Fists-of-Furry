using _Scripts;
using _Scripts.FSM_System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRagdollingState : FSMState
{
    private readonly NavMeshAgent _navigation;
    private readonly RagdollController _ragdollController;
    private readonly FSMSystem _fsm;
    private readonly EnemyConfig _enemyConfig;
    private readonly Transform _target;
    private float _ragdollTimer = 0;

    public EnemyRagdollingState(NavMeshAgent navigation, RagdollController ragdollController, FSMSystem fsm, EnemyConfig enemyConfig, Transform target)
    {
        _navigation = navigation;
        _ragdollController = ragdollController;
        _fsm = fsm;
        _enemyConfig = enemyConfig;
        _target = target;
        stateID = StateID.Ragdolling;
    }

    public override void Reason()
    {
        if (_ragdollTimer <= 0f)
        {
            _fsm.PerformTransition(Transition.ToDead);
        }
    }

    public override void Act()
    {
        _ragdollTimer -= Time.smoothDeltaTime;
        
    }

    public override void DoBeforeEntering()
    {
        _navigation.enabled = false;
        _ragdollController.RagdollActive(true);
        _ragdollController.ApplyExplosionForce(_enemyConfig.RagdollExplosionForce, _target.position, _enemyConfig.RagdollUpwardsModifier);
        _ragdollTimer = _enemyConfig.DestroyTimer;
    }
}