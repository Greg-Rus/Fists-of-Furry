using _Scripts.FSM_System;
using UnityEngine.AI;

public class PlayerBlockedByWrongInputState : FSMState
{
    private readonly NavMeshAgent _navigation;
    private readonly AnimationController _animation;
    private readonly PlayerConfig _playerConfig;

    public PlayerBlockedByWrongInputState(NavMeshAgent navigation, AnimationController animation, PlayerConfig playerConfig)
    {
        _navigation = navigation;
        _animation = animation;
        _playerConfig = playerConfig;
        stateID = StateID.BlockedByWrongInput;
    }

    public override void Reason()
    {
        //WaitForGotHitCallback
    }

    public override void Act()
    {
        //Stay still
    }

    public override void DoBeforeEntering()
    {
        _navigation.enabled = false;
        _animation.AnimationSpeed = _playerConfig.DefaultAnimationSpeed;
    }
}