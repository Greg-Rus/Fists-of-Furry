using System;
using Shapes;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class EnemyController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject _highlight;
        [SerializeField] private Disc _marker;
#pragma warning restore 649
        private NavMeshAgent _navigation;
        private RagdollController _ragdollController;
        private EnemyAI _ai;
        private EnemyConfig _config;
        private Transform _target;
        private Animator _animator;
        public EnemyAI AI => _ai;
        public int CurrentHitTypeIndex;
        public RegularPolygon[] HitPointDisplay;
        private HitTypes[] _hitOrder;
        private bool _isDead = false;


        public void Setup(EnemyConfig config, Transform target, HitTypes[] hitOrder)
        {
            _config = config;
            _target = target;
            _hitOrder = hitOrder;
            CurrentHitTypeIndex = hitOrder.Length - 1;
            SetupHitOrderDisplay();
            switch (hitOrder.Length)
            {
                case 1 : _marker.Color = Color.gray;
                    break;
                case 2 : _marker.Color = Color.yellow;
                    break;
                case 3 : _marker.Color = Color.red;
                    break;
                default: _marker.Color = Color.magenta;
                    break;
            }
        
            _ai.Setup(target, config, this);
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

        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        public void RegisterHit(HitTypes hitType)
        {
            if (IsDead) return;

            if (_hitOrder[CurrentHitTypeIndex] == HitTypes.Any ||
                _hitOrder[CurrentHitTypeIndex] == hitType)
            {
                HitPointDisplay[CurrentHitTypeIndex].enabled = false;
                CurrentHitTypeIndex--;
            }
            
            if (CurrentHitTypeIndex < 0)
            {
                IsDead = true;
                _highlight.SetActive(false);
                _marker.gameObject.SetActive(false);
            }
        }

        private void SetupHitOrderDisplay()
        {
            foreach (var display in HitPointDisplay)
            {
                display.enabled = false;
            }

            for (int i = 0; i < _hitOrder.Length; i++)
            {
                HitPointDisplay[i].enabled = true;
                SetHitType(HitPointDisplay[i], _hitOrder[i]);
            }
        }

        private void SetHitType(RegularPolygon display, HitTypes type)
        {
            switch (type)
            {
                case HitTypes.Any:
                    display.Sides = 4;
                    display.Color = Color.green;
                    break;
                case HitTypes.Punch:
                    display.Sides = 3;
                    display.Color = Color.yellow;
                    display.Angle = 1.5f;
                    break;
                case HitTypes.Kick:
                    display.Sides = 3;
                    display.Color = Color.blue;
                    display.Angle = 0.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
    }
}