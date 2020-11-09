using _Scripts.FSM_System;
using UnityEngine;
using UnityEngine.AI;

public class PunchingState : AttackingState
{
    private readonly AnimationController _animation;

    public PunchingState(FSMSystem fsm, TargetSelector targetSelector, AnimationController animation, UserInput userInput, PlayerConfig playerConfig, NavMeshAgent navigation, Transform playerTransform) 
        : base(fsm, targetSelector, animation, userInput, playerConfig, navigation, playerTransform)
    {
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
    }
}