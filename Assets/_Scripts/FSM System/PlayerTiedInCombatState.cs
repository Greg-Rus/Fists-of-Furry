using _Scripts.FSM_System;
using UnityEngine.AI;

public class PlayerTiedInCombatState : FSMState
{
    private FSMSystem _fsm;
    private UserInput _userInput;
    private TargetSelector _targetSelector;
    private readonly NavMeshAgent _navigation;

    public PlayerTiedInCombatState(FSMSystem fsm, UserInput userInput, TargetSelector targetSelector, NavMeshAgent navigation)
    {
        _fsm = fsm;
        _userInput = userInput;
        _targetSelector = targetSelector;
        _navigation = navigation;
        stateID = StateID.TiedInCombat;
    }

    public override void Reason()
    {

        switch (_userInput.LastAttackInput)
        {
            case AttackType.Punch:
                _fsm.PerformTransition(Transition.ToPunching);
                break;
            case AttackType.Kick:
                _fsm.PerformTransition(Transition.ToKicking);
                break;
        }

        //TODO: Disengage mechanic.
    }

    public override void Act()
    {
        //Just wait for input.
    }

    public override void DoBeforeEntering()
    {
        _navigation.enabled = false;
    }
}