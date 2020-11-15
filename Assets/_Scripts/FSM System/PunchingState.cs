using _Scripts.FSM_System;
using UnityEngine;
using UnityEngine.AI;

public class PunchingState : PlayerAttackingState
{
    private readonly FSMSystem _fsm;
    private readonly TargetSelector _targetSelector;
    private readonly AnimationController _animation;

    public PunchingState(FSMSystem fsm, TargetSelector targetSelector, AnimationController animation, UserInput userInput, PlayerConfig playerConfig, NavMeshAgent navigation, Transform playerTransform) 
        : base(fsm, targetSelector, animation, userInput, playerConfig, navigation, playerTransform)
    {
        _fsm = fsm;
        _targetSelector = targetSelector;
        _animation = animation;
        stateID = StateID.Punching;
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();

        var punchId = UnityEngine.Random.Range(1, 4);
        var punchSide = LastAttackSide == AttackSide.Left ? AttackSide.Right : AttackSide.Left;
        LastAttackSide = punchSide;

        _animation.StartPunchAnimation(punchId, punchSide);
        
        var wasCorrectHitType = _targetSelector.EnemyCurrentlyInCombat.CheckIfHitTypeCorrect(HitTypes.Punch);
        if(!wasCorrectHitType) _fsm.PerformTransition(Transition.ToBlockedByWrongInput);
    }
}