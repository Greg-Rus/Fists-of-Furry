using System;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private GameObject _highlight;

        private NavMeshAgent _navigation;
        private RagdollController _ragdollController;
        private EnemyAI _ai;
        private EnemyConfig _config;
        private Transform _target;
        private Animator _animator;
        public EnemyAI AI => _ai;


        public void Setup(EnemyConfig config, Transform target)
        {
            _config = config;
            _target = target;
        
            _ai.Setup(target, config);
        }
    
        private void OnEnable()
        {
            _navigation = GetComponent<NavMeshAgent>();
            _ragdollController = GetComponent<RagdollController>();
            _ai = GetComponent<EnemyAI>();
            _animator = GetComponent<Animator>();
        }
        
        public bool IsHighlighted
        {
            set => _highlight.SetActive(value);
            get => _highlight.activeSelf;
        }
        

    }
}