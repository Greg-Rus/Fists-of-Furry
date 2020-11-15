using _Scripts.FSM_System;

public class EnemyBlockingState : FSMState
{
    private readonly AnimationController _animation;

    public EnemyBlockingState(AnimationController animation)
    {
        _animation = animation;
        stateID = StateID.Blocking;
    }

    public override void Reason()
    {
        
    }

    public override void Act()
    {
        
    }

    public override void DoBeforeEntering()
    {
        _animation.IsBlocking = true;
    }

    public override void DoBeforeLeaving()
    {
        _animation.IsBlocking = false;
    }
}