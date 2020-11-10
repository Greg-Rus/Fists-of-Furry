using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.FSM_System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterStates
{
    Walking,
    FollowingAtDistance,
    Charging,
    TiedInCombat,
    Attacking,
    WindingDownFromAttack,
    GettingHit,
    Ragdolling,
    Dead
}

public class PlayerController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private TargetSelector _targetSelector;
    [SerializeField] private UserInput _userInput;
#pragma warning restore 649
    private AnimationController _animation;
    public CharacterStates State = CharacterStates.Walking;

    private NavMeshAgent _navigation;
    
    [CanBeNull] private FSMSystem _fsm;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _navigation = GetComponent<NavMeshAgent>();
        _animation = GetComponent<AnimationController>();
        _animation.OnHit += OnHitEnemyCallback;
        _animation.AddAttackCompletedCallback(OnAttackCompetedCallback);
        
        _fsm = new FSMSystem();
        var walkingState = new WalkingState(_fsm, _navigation, _userInput, _targetSelector, _playerConfig, _animation);
        walkingState.AddTransition(Transition.ToCharging, StateID.Charging);
        
        var chargingState = new ChargingState(_fsm, _navigation, this, _playerConfig, _animation, _targetSelector, transform);
        chargingState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat);

        var punchingState = new PunchingState(_fsm, _targetSelector, _animation, _userInput, _playerConfig, _navigation, transform);
        punchingState.AddTransition(Transition.ToWindingDownFromAttack, StateID.WindingDownFromAttack);
        
        var kickingState = new KickingState(_fsm, _targetSelector, _animation, _userInput, _playerConfig, _navigation, transform);
        kickingState.AddTransition(Transition.ToWindingDownFromAttack, StateID.WindingDownFromAttack);

        var tiedInCombatState = new TiedInCombatState(_fsm, _userInput, _targetSelector, _navigation);
        tiedInCombatState.AddTransition(Transition.ToPunching, StateID.Punching);
        tiedInCombatState.AddTransition(Transition.ToKicking, StateID.Kicking);

        var windingDownState = new WindingDownFromAttackState(_fsm, _targetSelector, _userInput, _animation, _playerConfig);
        windingDownState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat); //no input
        windingDownState.AddTransition(Transition.ToWalking, StateID.Walking); //no input, enemy dead
        windingDownState.AddTransition(Transition.ToPunching, StateID.Punching); //new input enemy not dead
        windingDownState.AddTransition(Transition.ToKicking, StateID.Kicking); //new input enemy not dead
        windingDownState.AddTransition(Transition.ToCharging, StateID.Charging); //new input enemy dead
        
        _fsm.AddState(walkingState);
        _fsm.AddState(chargingState);
        _fsm.AddState(punchingState);
        _fsm.AddState(kickingState);
        _fsm.AddState(tiedInCombatState);
        _fsm.AddState(windingDownState);
    }
    
    // Update is called once per frame
    void Update()
    {
        Debug.Log(_fsm.CurrentState);
        _fsm.CurrentState.Reason();
        _fsm.CurrentState.Act();
    }
    
    #region Callback

    private void OnAttackCompetedCallback()
    {
        if (_targetSelector.EnemyCurrentlyInCombat == null)
        {
            _fsm.PerformTransition(Transition.ToWalking);
        }
        else if(_fsm.CurrentStateID == StateID.WindingDownFromAttack)
        {
            _fsm.PerformTransition(Transition.ToTiedInCombat);
        }
    }
    
    private void OnHitEnemyCallback()
    {
        var hitType = _fsm.CurrentStateID == StateID.Punching ? HitTypes.Punch : HitTypes.Kick;
        _targetSelector.EnemyCurrentlyInCombat.AI.OnHitConnect(hitType);
        if (_targetSelector.EnemyCurrentlyInCombat.AI.State == CharacterStates.Ragdolling ||
            _targetSelector.EnemyCurrentlyInCombat.AI.State == CharacterStates.Dead)
        {
            _targetSelector.EnemyCurrentlyInCombat = null;
        }
        
        _fsm.PerformTransition(Transition.ToWindingDownFromAttack);
    }

    #endregion
}
