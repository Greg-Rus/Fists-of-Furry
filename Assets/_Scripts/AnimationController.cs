using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    public delegate void OnHitDelegate();
    public OnHitDelegate OnHit;
    
    private Animator _animator;
    private AttackBehaviour[] _attackBehaviours;
    private HitRecoilBehaviour[] _hitRecoilBehaviours;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _attackBehaviours = _animator.GetBehaviours<AttackBehaviour>();
        _hitRecoilBehaviours = _animator.GetBehaviours<HitRecoilBehaviour>();

    }

    public void AddAttackCompletedCallback(Action callback)
    {
        foreach (var behaviour in _attackBehaviours)
        {
            behaviour.AttackCompletedDelegate += callback.Invoke;
        }
    }   
    
    public void AddHitRecoilCallback(Action callback)
    {
        foreach (var behaviour in _hitRecoilBehaviours)
        {
            behaviour.HitRecoilCompletedDelegate += callback.Invoke;
        }
    }

    public bool IsCharging
    {
        get => _animator.GetBool(AnimatorProperties.Charge);
        set => _animator.SetFloat(AnimatorProperties.Charge, value ? 1f : 0f);
    }

    public bool IsBlocking
    {
        set => _animator.SetBool(AnimatorProperties.Blocking, value);
    }

    public void PlayBlockRecoil()
    {
        _animator.SetTrigger(AnimatorProperties.BlockRecoil);
    }

    public void StartPunchAnimation(int animationIndex, AttackSide attackSide)
    {
        _animator.SetInteger(AnimatorProperties.PunchNumber, animationIndex);
        _animator.SetInteger(AnimatorProperties.AttackSide, (int)attackSide);
        _animator.SetTrigger(AnimatorProperties.PunchTrigger);
    }
    
    public void StartKickAnimation(int animationIndex, AttackSide attackSide)
    {
        _animator.SetInteger(AnimatorProperties.KickNumber, animationIndex);
        _animator.SetInteger(AnimatorProperties.AttackSide, (int)attackSide);
        _animator.SetTrigger(AnimatorProperties.KickTrigger);
    }

    public void StartGotHitAnimation(int animationIndex)
    {
        _animator.SetInteger(AnimatorProperties.HitDirection, animationIndex);
        _animator.SetTrigger(AnimatorProperties.GetHitTrigger);
    }

    public float AnimationSpeed
    {
        set => _animator.speed = value;
    }

    public bool ApplyRootMotion
    {
        set => _animator.applyRootMotion = value;
    }
    
    public void Hit()
    {
        OnHit.Invoke();
    }
    
    public void FootR()
    {
    }

    public void FootL()
    {
    }
}