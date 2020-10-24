using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterStates
{
    Walking,
    FollowingAtDistance,
    Charging,
    Attacking,
    GettingHit,
    Ragdolling,
    Dead
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private UserInput _userInput;
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private TargetSelector _targetSelector;
    private AnimationController _animation;
    public CharacterStates State = CharacterStates.Walking;

    private NavMeshAgent _navigation;

    private EnemyController _enemyInCombat;
    private AttackType _attackInput;

    private AttackSide _lastAttackSide = AttackSide.Left;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _navigation = GetComponent<NavMeshAgent>();
        _animation = GetComponent<AnimationController>();
        _animation.OnHit += HitEnemy;
        _animation.AddAttackCallback(TransitionToWalking);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case CharacterStates.Walking:
                UpdateWalking();
                break;
            case CharacterStates.Charging:
                UpdateCharging();
                break;
            case CharacterStates.Attacking:
                UpdateAttacking();
                break;
            case CharacterStates.GettingHit:
                break;
            case CharacterStates.Ragdolling:
                break;
            case CharacterStates.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateWalking()
    {
        if (Input.GetMouseButtonDown(0) && _targetSelector.SelectedTarget != null)
        {
            TransitionToCharging();
            _attackInput = AttackType.Punch;
        }
        if (Input.GetMouseButtonDown(1) && _targetSelector.SelectedTarget != null)
        {
            TransitionToCharging();
            _attackInput = AttackType.Kick;
        }
        
        _navigation.SetDestination(_userInput.InputDestination);
    }

    private void TransitionToCharging()
    {
        _navigation.speed = _playerConfig.ChargingSpeed;
        _animation.IsCharging = true;
        _enemyInCombat = _targetSelector.SelectedTarget;
        State = CharacterStates.Charging;
    }

    private void UpdateCharging()
    {
        _navigation.SetDestination(_enemyInCombat.transform.position);
        
        if (_navigation.remainingDistance <= 1.1f)
        {
            TransitionToAttacking();
        }
    }

    private void TransitionToAttacking()
    {
        _navigation.ResetPath();
        _navigation.enabled = false;
        _animation.IsCharging = false;
        _enemyInCombat.AI.OnUnderAttack();
        State = CharacterStates.Attacking;
        
        switch (_attackInput)
        {
            case AttackType.Punch: ExecutePunch();
                break;
            case AttackType.Kick: ExecuteKick();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    private void UpdateAttacking()
    {
        //wait for animation delegate callback;
    }

    private void TransitionToWalking()
    {
        _navigation.enabled = true;
        _navigation.speed = _playerConfig.WalkingSpeed;
        State = CharacterStates.Walking;
    }

    private void HitEnemy()
    {
        _enemyInCombat.AI.OnHitConnect();
    }

    private void ExecutePunch()
    {
        var punchId = UnityEngine.Random.Range(1, 4);
        var punchSide = _lastAttackSide == AttackSide.Left ? AttackSide.Right : AttackSide.Left;
        _lastAttackSide = punchSide;
        
        _animation.StartPunchAnimation(punchId, punchSide);
    }    
    
    private void ExecuteKick()
    {
        var kickId = UnityEngine.Random.Range(1, 3);
        var punchSide = _lastAttackSide == AttackSide.Left ? AttackSide.Right : AttackSide.Left;
        _lastAttackSide = punchSide;
        _animation.StartKickAnimation(kickId, punchSide);
    }
    
}
