using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.FSM_System;
using JetBrains.Annotations;
using UniRx;
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
        _animation.AddHitRecoilCallback(OnHitRecoilCompleted);
        
        _fsm = new FSMSystem();
        var walkingState = new PlayerWalkingState(_fsm, _navigation, _userInput, _targetSelector, _playerConfig, _animation);
        walkingState.AddTransition(Transition.ToCharging, StateID.Charging);
        
        var chargingState = new PlayerChargingState(_fsm, _navigation, this, _playerConfig, _animation, _targetSelector, transform);
        chargingState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat);

        var punchingState = new PunchingState(_fsm, _targetSelector, _animation, _userInput, _playerConfig, _navigation, transform);
        punchingState.AddTransition(Transition.ToWindingDownFromAttack, StateID.WindingDownFromAttack);
        punchingState.AddTransition(Transition.ToBlockedByWrongInput, StateID.BlockedByWrongInput);
        
        var kickingState = new KickingState(_fsm, _targetSelector, _animation, _userInput, _playerConfig, _navigation, transform);
        kickingState.AddTransition(Transition.ToWindingDownFromAttack, StateID.WindingDownFromAttack);
        kickingState.AddTransition(Transition.ToBlockedByWrongInput, StateID.BlockedByWrongInput);

        var tiedInCombatState = new PlayerTiedInCombatState(_fsm, _userInput, _targetSelector, _navigation);
        tiedInCombatState.AddTransition(Transition.ToPunching, StateID.Punching);
        tiedInCombatState.AddTransition(Transition.ToKicking, StateID.Kicking);
        
        var windingDownState = new PlayerWindingDownFromAttackState(_fsm, _targetSelector, _userInput, _animation, _playerConfig);
        windingDownState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat); //no input
        windingDownState.AddTransition(Transition.ToWalking, StateID.Walking); //no input, enemy dead
        windingDownState.AddTransition(Transition.ToPunching, StateID.Punching); //new input enemy not dead
        windingDownState.AddTransition(Transition.ToKicking, StateID.Kicking); //new input enemy not dead
        windingDownState.AddTransition(Transition.ToCharging, StateID.Charging); //new input enemy dead
        windingDownState.AddTransition(Transition.ToGettingHit, StateID.GettingHit);
        
        var blockByWrongInput = new PlayerBlockedByWrongInputState(_navigation, _animation, _playerConfig);
        blockByWrongInput.AddTransition(Transition.ToGettingHit, StateID.GettingHit);
        
        var gettingHitState = new EnemyGettingHitState(_navigation, _animation);
        gettingHitState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat);
        
        _fsm.AddState(walkingState);
        _fsm.AddState(chargingState);
        _fsm.AddState(punchingState);
        _fsm.AddState(kickingState);
        _fsm.AddState(tiedInCombatState);
        _fsm.AddState(windingDownState);
        _fsm.AddState(gettingHitState);
        _fsm.AddState(blockByWrongInput);

        //_fsm.CurrentStateIdRx.Subscribe(state => Debug.Log("Player State: " + state));
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.CurrentState.Reason();
        _fsm.CurrentState.Act();
    }
    
    #region Callback

    private void OnAttackCompetedCallback()
    {
        if (_targetSelector.EnemyCurrentlyInCombat == null && 
            _fsm.CurrentStateID == StateID.WindingDownFromAttack)
        {
            _fsm.PerformTransition(Transition.ToWalking);
        }
        else 
        if(_fsm.CurrentStateID == StateID.WindingDownFromAttack)
        {
            _fsm.PerformTransition(Transition.ToTiedInCombat);
        }
    }
    
    private void OnHitEnemyCallback()
    {
        var hitType = _fsm.CurrentStateID == StateID.Punching ? HitTypes.Punch : HitTypes.Kick;
        _targetSelector.EnemyCurrentlyInCombat.OnHitConnect(hitType);
        // if (_targetSelector.EnemyCurrentlyInCombat.State == StateID.Ragdolling ||
        //     _targetSelector.EnemyCurrentlyInCombat.State == StateID.Dead)
        // {
        //     _targetSelector.EnemyCurrentlyInCombat = null;
        // }

        if (_fsm.CurrentStateID != StateID.BlockedByWrongInput)
        {
            _fsm.PerformTransition(Transition.ToWindingDownFromAttack);
        }
    }

    public void OnGotHitByEnemy()
    {
        _fsm.PerformTransition(Transition.ToGettingHit);
    }
    
    private void OnHitRecoilCompleted()
    {
        _fsm.PerformTransition(Transition.ToTiedInCombat);
    }

    #endregion
}
