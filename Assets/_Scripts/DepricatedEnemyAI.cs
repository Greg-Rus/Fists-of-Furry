﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class DepricatedEnemyAI : MonoBehaviour
    {
        private Transform _target;
        private NavMeshAgent _navigation;
        private RagdollController _ragdollController;
        private EnemyConfig _config;
        private AnimationController _animationController;
        public CharacterStates State = CharacterStates.FollowingAtDistance;
        // private float _ragdollTimer = 0f;
        private EnemyController _controller;
    
        public void Setup(Transform target, EnemyConfig config, EnemyController controller)
        {
            _target = target;
            _config = config;
            _navigation.speed = _config.WalkingSpeed;
            _controller = controller;
        }
        void OnEnable()
        {
            _navigation = GetComponent<NavMeshAgent>();
            _navigation.updateRotation = false;

            // _animationController = GetComponent<AnimationController>();
            // _animationController.AddHitRecoilCallback(OnHitRecoilEndedCallback);

            _ragdollController = GetComponent<RagdollController>();
            _ragdollController.RagdollActive(false);
        }
        
        // private void Update()
        // {
        //     switch (State)
        //     {
        //         // case CharacterStates.FollowingAtDistance: UpdateFollowAtDistance();
        //         //     break;
        //         // case CharacterStates.GettingHit : UpdateGettingHit();
        //         //     break; 
        //         // case CharacterStates.Ragdolling : UpdateRagdolling();
        //         //     break;
        //         // case CharacterStates.TiedInCombat : UpdateTiedInCombat();
        //         //     break;
        //     }
        // }

        // private Vector3 DesiredPosition
        // {
        //     get
        //     {
        //         var enemyPosition = transform.position;
        //         var targetPosition = _target.position;
        //         var directionFromTarget = enemyPosition - targetPosition;
        //         return (directionFromTarget.normalized * _config.AttackRange) + targetPosition;
        //     }
        // }

        // public void OnUnderAttack()
        // {
        //     TransitionToTiedInCombat();
        // }

        // public void OnHitConnect(HitTypes hitType)
        // {
        //     _controller.RegisterHit(hitType);
        //     if(_controller.IsDead) TransitionToRagdolling();
        //     else
        //     {
        //         var hitAnimationIndex = UnityEngine.Random.Range(1, 4);
        //         _animationController.StartGotHitAnimation(hitAnimationIndex);
        //         TransitionToGettingHit();
        //     }
        // }

        // private void OnHitRecoilEndedCallback()
        // {
        //     TransitionToTiedInCombat();
        // }
        
        // private void TransitionToFollowAtDistance()
        // {
        //     _navigation.enabled = true;
        //     State = CharacterStates.FollowingAtDistance;
        // }
        
        // private void UpdateFollowAtDistance()
        // {
        //     _navigation.SetDestination(DesiredPosition);
        //     transform.LookAt(_target, Vector3.up);
        // }

        // private void TransitionToGettingHit()
        // {
        //     _navigation.enabled = false;
        //     State = CharacterStates.GettingHit;
        // }

        // private void UpdateGettingHit()
        // {
        //     //Wait for OnHitCall
        // }

        // private void TransitionToRagdolling()
        // {
        //     _navigation.enabled = false;
        //     _ragdollController.RagdollActive(true);
        //     _ragdollController.ApplyExplosionForce(_config.RagdollExplosionForce, _target.position, _config.RagdollUpwardsModifier);
        //     _ragdollTimer = _config.DestroyTimer;
        //     State = CharacterStates.Ragdolling;
        // }

        // private void UpdateRagdolling()
        // {
        //     _ragdollTimer -= Time.smoothDeltaTime;
        //     if (_ragdollTimer <= 0f) TransitionToDead();
        // }

        // private void TransitionToDead()
        // {
        //     State = CharacterStates.Dead;
        //     Destroy(gameObject, 2f);
        // }

        // public void TransitionToTiedInCombat()
        // {
        //     _navigation.enabled = false;
        //     State = CharacterStates.TiedInCombat;
        // }
        //
        // private void UpdateTiedInCombat()
        // {
        //     //Timer to execute attack
        // }
    }
}