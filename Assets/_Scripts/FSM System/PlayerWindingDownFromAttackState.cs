using System;
using _Scripts.FSM_System;

public class PlayerWindingDownFromAttackState : FSMState
{
    private FSMSystem _fsm;
    private readonly UserInput _userInput;
    private readonly TargetSelector _targetSelector;
    private readonly PlayerConfig _playerConfig;
    private readonly AnimationController _animation;
    public PlayerWindingDownFromAttackState(FSMSystem fsm, TargetSelector targetSelector, UserInput userInput, AnimationController animation, PlayerConfig playerConfig)
    {
        _fsm = fsm;
        _targetSelector = targetSelector;
        _userInput = userInput;
        _animation = animation;
        _playerConfig = playerConfig;
        stateID = StateID.WindingDownFromAttack;
    }

    public override void Reason()
    {
        if (_userInput.LastAttackInput != AttackType.None && 
            _targetSelector.EnemyCurrentlyInCombat != null)
        {
            switch (_userInput.LastAttackInput)
            {
                case AttackType.Punch:
                    _fsm.PerformTransition(Transition.ToPunching);
                    break;
                case AttackType.Kick:
                    _fsm.PerformTransition(Transition.ToKicking);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (_userInput.LastAttackInput != AttackType.None && 
            _targetSelector.EnemyCurrentlyInCombat == null &&
            _targetSelector.SelectedTarget != null)
        {
            _fsm.PerformTransition(Transition.ToCharging);
                
        }
    }

    public override void Act()
    {
        
    }

    public override void DoBeforeEntering()
    {
        _animation.AnimationSpeed = _playerConfig.AttackWindDownAnimationSpeed;
    }
}