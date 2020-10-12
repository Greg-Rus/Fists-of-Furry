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
        }

        public bool IsHighlighted
        {
            set => _highlight.SetActive(value);
        }
    
        public void OnGotHit()
        {
        
        }

        public void OnDied()
        {
            _navigation.enabled = false;
            _ai.enabled = false;
            _ragdollController.RagdollActive(true);
            _ragdollController.ApplyExplosionForce(_config.RagdollExplosionForce, _target.position, _config.RagdollUpwardsModifier);
        
            Destroy(gameObject, 5f);
        }
    }
}