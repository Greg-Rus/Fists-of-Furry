using _Scripts.FSM_System;
using UnityEngine.AI;

public class EnemyGettingHitState : FSMState
{
    private readonly NavMeshAgent _navigation;
    private readonly AnimationController _animationController;

    public EnemyGettingHitState(NavMeshAgent navigation, AnimationController animationController)
    {
        _navigation = navigation;
        _animationController = animationController;
        stateID = StateID.GettingHit;
    }

    public override void Reason()
    {
        //Wait for external callback
    }

    public override void Act()
    {
        //Wait for external callback
    }

    public override void DoBeforeEntering()
    {
        _navigation.enabled = false;
        var hitAnimationIndex = UnityEngine.Random.Range(1, 4);
        _animationController.StartGotHitAnimation(hitAnimationIndex);
    }
}