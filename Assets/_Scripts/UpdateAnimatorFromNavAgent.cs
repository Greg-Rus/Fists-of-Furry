using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class UpdateAnimatorFromNavAgent : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private bool _isCharging;

    public bool IsCharging
    {
        get => _isCharging;
        set
        {
            _isCharging = value;
            _animator.SetFloat(AnimatorProperties.Charge,  _isCharging ? 1f : 0f);
        }
    }

    void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_navMeshAgent.velocity.sqrMagnitude > 0.001f)
        {       
            var relativeVelocity = transform.InverseTransformDirection(_navMeshAgent.velocity);
            _animator.SetFloat(AnimatorProperties.VeloctyX, relativeVelocity.x);
            _animator.SetFloat(AnimatorProperties.VeloctyZ, relativeVelocity.z);
        }
    }
}