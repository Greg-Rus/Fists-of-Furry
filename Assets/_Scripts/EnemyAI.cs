using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        private Transform _target;
        private NavMeshAgent _navigation;
        private RagdollController _ragdollController;
        private EnemyConfig _config;
        private AnimationController _animationController;
        public CharacterStates State = CharacterStates.FollowingAtDistance;
        private float _ragdollTimer = 0f;
    
        public void Setup(Transform target, EnemyConfig config)
        {
            _target = target;
            _config = config;
        }
        void Start()
        {
            _navigation = GetComponent<NavMeshAgent>();
            _navigation.speed = _config.WalkingSpeed;
            _navigation.updateRotation = false;

            _animationController = GetComponent<AnimationController>();

            _ragdollController = GetComponent<RagdollController>();
            _ragdollController.RagdollActive(false);
        }
        
        private void Update()
        {
            switch (State)
            {
                case CharacterStates.FollowingAtDistance: UpdateFollowAtDistance();
                    break;
                case CharacterStates.GettingHit : UpdateGettingHit();
                    break; 
                case CharacterStates.Ragdolling : UpdateRagdolling();
                    break;
            }
        }

        private Vector3 DesiredPosition
        {
            get
            {
                var enemyPosition = transform.position;
                var targetPosition = _target.position;
                var directionFromTarget = enemyPosition - targetPosition;
                return (directionFromTarget.normalized * _config.AttackRange) + targetPosition;
            }
        }

        public void OnUnderAttack()
        {
            TransitionToGettingHit();
        }

        public void OnHitConnect()
        {
            TransitionToRagdolling();
        }
        
        private void TransitionToFollowAtDistance()
        {
            _navigation.enabled = true;
            State = CharacterStates.FollowingAtDistance;
        }
        
        private void UpdateFollowAtDistance()
        {
            _navigation.SetDestination(DesiredPosition);
            transform.LookAt(_target, Vector3.up);
        }

        private void TransitionToGettingHit()
        {
            _navigation.enabled = false;
            State = CharacterStates.GettingHit;
        }

        private void UpdateGettingHit()
        {
            //Wait for OnHitCall
        }

        private void TransitionToRagdolling()
        {
            _navigation.enabled = false;
            _ragdollController.RagdollActive(true);
            _ragdollController.ApplyExplosionForce(_config.RagdollExplosionForce, _target.position, _config.RagdollUpwardsModifier);
            _ragdollTimer = _config.DestroyTimer;
            State = CharacterStates.Ragdolling;
        }

        private void UpdateRagdolling()
        {
            _ragdollTimer -= Time.smoothDeltaTime;
            if (_ragdollTimer <= 0f) TransitionToDead();
        }

        private void TransitionToDead()
        {
            State = CharacterStates.Dead;
            Destroy(gameObject, 2f);
        }
    }
}