using _Scripts.FSM_System;
using UnityEngine;

public class EnemyDeadState : FSMState
{
    private readonly GameObject _gameObject;

    public EnemyDeadState(GameObject gameObject)
    {
        _gameObject = gameObject;
        stateID = StateID.Dead;
    }

    public override void Reason()
    {
    }

    public override void Act()
    {
    }

    public override void DoBeforeEntering()
    {
        GameObject.Destroy(_gameObject, 2f);
    }
}