using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterStates
{
    Walking,
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
    private UpdateAnimatorFromNavAgent _animation;
    public CharacterStates State = CharacterStates.Walking;

    private NavMeshAgent _navigation;

    private EnemyController _enemyInCombat;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _navigation = GetComponent<NavMeshAgent>();
        _animation = GetComponent<UpdateAnimatorFromNavAgent>();
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
        State = CharacterStates.Attacking;
    }

    private void UpdateAttacking()
    {
        _enemyInCombat.OnDied();
        _enemyInCombat = null;
        TransitionToWalking();
    }

    private void TransitionToWalking()
    {
        _navigation.enabled = true;
        _navigation.speed = _playerConfig.WalkingSpeed;
        State = CharacterStates.Walking;
    }
}
