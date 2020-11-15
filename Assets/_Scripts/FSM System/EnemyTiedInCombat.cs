using _Scripts.FSM_System;
using UnityEngine.AI;

public class EnemyTiedInCombat : FSMState
{
    private readonly NavMeshAgent _navigation;

    public EnemyTiedInCombat(NavMeshAgent navigation)
    {
        _navigation = navigation;
        stateID = StateID.TiedInCombat;
    }

    public override void Reason()
    {
    }

    public override void Act()
    {
    }

    public override void DoBeforeEntering()
    {
        _navigation.enabled = false;
    }
}