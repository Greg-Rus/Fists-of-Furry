using _Scripts.FSM_System;
using UnityEngine;
using UnityEngine.AI;

public class KickingState : PlayerAttackingState
{
    private readonly FSMSystem _fsm;
    private readonly TargetSelector _targetSelector;
    private readonly AnimationController _animation;

    public KickingState(FSMSystem fsm, TargetSelector targetSelector, AnimationController animation, UserInput userInput, PlayerConfig playerConfig, NavMeshAgent navigation, Transform playerTransform) 
        : base(fsm, targetSelector, animation, userInput, playerConfig, navigation, playerTransform)
    {
        _fsm = fsm;
        _targetSelector = targetSelector;
        _animation = animation;
        stateID = StateID.Kicking;
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();

        var kickId = UnityEngine.Random.Range(1, 3);
        var punchSide = LastAttackSide == AttackSide.Left ? AttackSide.Right : AttackSide.Left;
        LastAttackSide = punchSide;

        _animation.StartKickAnimation(kickId, punchSide);
        
        var wasCorrectHitType = _targetSelector.EnemyCurrentlyInCombat.CheckIfHitTypeCorrect(HitTypes.Kick);
        if(!wasCorrectHitType) _fsm.PerformTransition(Transition.ToBlockedByWrongInput);
    }
}