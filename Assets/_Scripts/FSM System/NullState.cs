using _Scripts.FSM_System;

public class NullState : FSMState
{
    public NullState()
    {
        stateID = StateID.NullStateID;
    }

    public override void Reason()
    {
        throw new System.NotImplementedException();
    }

    public override void Act()
    {
        throw new System.NotImplementedException();
    }
}