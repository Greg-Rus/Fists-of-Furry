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

            _ragdollController = GetComponent<RagdollController>();
            _ragdollController.RagdollActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            _navigation.SetDestination(DesiredPosition);
            transform.LookAt(_target, Vector3.up);
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

        public void OnGotHit()
        {
        
        }

        public void OnDied()
        {
            _navigation.enabled = false;
            _ragdollController.RagdollActive(true);
            _ragdollController.ApplyExplosionForce(_config.RagdollExplosionForce, _target.position, _config.RagdollUpwardsModifier);
        
        }
    }
}