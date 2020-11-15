using System;
using _Scripts.FSM_System;
using Shapes;
using UniRx;
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
        private EnemyConfig _config;
        private Transform _target;
        public int CurrentHitTypeIndex;
        public RegularPolygon[] HitPointDisplay;
        public Torus[] HitPointTorus;
        public Rectangle[] HitPointRectangles;
        private HitTypes[] _hitOrder;
        private bool _isDead = false;

        private FSMSystem _fsm;
        private AnimationController _animationController;
        private PlayerController _playerController;
        public Action OnDied;
        
        private void OnEnable()
        {
            _navigation = GetComponent<NavMeshAgent>();
            _ragdollController = GetComponent<RagdollController>();
            _animationController = GetComponent<AnimationController>();
            _animationController.OnHit += OnHitEnemyCallback;
            _animationController.AddAttackCompletedCallback(OnAttackCompetedCallback);
        }

        private void OnAttackCompetedCallback()
        {
            if (_fsm.CurrentStateID == StateID.Attacking)
            {
                _fsm.PerformTransition(Transition.ToTiedInCombat);
            }
        }

        private void OnHitEnemyCallback()
        {
            _playerController.OnGotHitByEnemy();
        }

        public void Setup(EnemyConfig config, PlayerController player, HitTypes[] hitOrder)
        {
            _config = config;
            _playerController = player;
            _target = player.transform;
            _hitOrder = hitOrder;
            CurrentHitTypeIndex = hitOrder.Length - 1;
            SetupHitOrderDisplay();
            SetupFsm();
        }
        
        private void SetupFsm()
        {
            _fsm = new FSMSystem();

            var followState = new EnemyFollowAtDistanceState(_fsm, _target, transform, _config, _navigation);
            followState.AddTransition(Transition.ToGettingHit, StateID.GettingHit);
            followState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat);

            var gettingHitState = new EnemyGettingHitState(_navigation, _animationController);
            gettingHitState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat);

            var tiedInCombat = new EnemyTiedInCombat(_navigation);
            tiedInCombat.AddTransition(Transition.ToGettingHit, StateID.GettingHit);
            tiedInCombat.AddTransition(Transition.ToRagdolling, StateID.Ragdolling);
            tiedInCombat.AddTransition(Transition.ToBlocking, StateID.Blocking);
            tiedInCombat.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat);

            var enemyRagdollingState = new EnemyRagdollingState(_navigation, _ragdollController, _fsm, _config, _target);
            enemyRagdollingState.AddTransition(Transition.ToDead, StateID.Dead);
            
            var enemyBlockingState = new EnemyBlockingState(_animationController);
            //enemyBlockingState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat);
            enemyBlockingState.AddTransition(Transition.ToAttacking, StateID.Attacking);
            
            var enemyAttackingState = new EnemyAttackingState(_target, transform, _animationController, _navigation, _config);
            enemyAttackingState.AddTransition(Transition.ToTiedInCombat, StateID.TiedInCombat);

            _fsm.AddState(followState);
            _fsm.AddState(gettingHitState);
            _fsm.AddState(tiedInCombat);
            _fsm.AddState(enemyRagdollingState);
            _fsm.AddState(new EnemyDeadState(gameObject));
            _fsm.AddState(enemyBlockingState);
            _fsm.AddState(enemyAttackingState);

            _animationController.AddHitRecoilCallback(() =>
            {
                if (_fsm.CurrentStateID == StateID.GettingHit)
                {
                    _fsm.PerformTransition(Transition.ToTiedInCombat);    
                }
            });
            
            //_fsm.CurrentStateIdRx.Subscribe(state => Debug.Log("Enemy State: " + state));
        }

        void Update()
        {
            _fsm.CurrentState.Reason();
            _fsm.CurrentState.Act();
        }

        public bool CheckIfHitTypeCorrect(HitTypes hitType)
        {
            var isCorrectHitType = IncomingHitCorrect(hitType);
            if (!isCorrectHitType)
            {
                _fsm.PerformTransition(Transition.ToBlocking);
            }

            return isCorrectHitType;
        }
        
        public void OnHitConnect(HitTypes hitType)
        {
            if (_fsm.CurrentStateID == StateID.Blocking)
            {
                _animationController.PlayBlockRecoil(); //TODO: add separate state so that this has time to play out?
                _fsm.PerformTransition(Transition.ToAttacking); 
            }
            else
            {
                RegisterHit(hitType);
                if (IsDead)
                {
                    _fsm.PerformTransition(Transition.ToRagdolling);
                }
                else
                {
                    _fsm.PerformTransition(Transition.ToGettingHit);
                }
            }
        }
        
        public bool IsHighlighted
        {
            set => _highlight.SetActive(value);
        }

        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        private void RegisterHit(HitTypes hitType)
        {
            if (IsDead) throw new Exception("A dead enemy was hit");

            if (IncomingHitCorrect(hitType))
            {
                HitPointDisplay[CurrentHitTypeIndex].enabled = false;
                HitPointTorus[CurrentHitTypeIndex].enabled = false;
                HitPointRectangles[CurrentHitTypeIndex].enabled = false;
                CurrentHitTypeIndex--;
            }
            
            if (CurrentHitTypeIndex < 0)
            {
                IsDead = true;
                _highlight.SetActive(false);
                _marker.gameObject.SetActive(false);
                OnDied?.Invoke();
            }
        }

        private void SetupHitOrderDisplay()
        {
            foreach (var display in HitPointDisplay)
            {
                display.enabled = false;
            }
            
            foreach (var display in HitPointTorus)
            {
                display.enabled = false;
            }            
            
            foreach (var display in HitPointRectangles)
            {
                display.enabled = false;
            }

            // for (int i = 0; i < _hitOrder.Length; i++)
            // {
            //     HitPointDisplay[i].enabled = true;
            //     SetHitType(HitPointDisplay[i], _hitOrder[i]);
            // }

            // for (int i = 0; i < _hitOrder.Length; i++)
            // {
            //     HitPointTorus[i].enabled = true;
            //     HitPointTorus[i].Color = GetHitTypeColor(_hitOrder[i]).WithAlpha(0.5f);
            // }

            for (int i = 0; i < _hitOrder.Length; i++)
            {
                HitPointRectangles[i].enabled = true;
                HitPointRectangles[i].Color = GetHitTypeColor(_hitOrder[i]).WithAlpha(0.5f);
            }
            
            switch (_hitOrder.Length)
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
        }

        private void SetHitType(RegularPolygon display, HitTypes type)
        {
            switch (type)
            {
                case HitTypes.Any:
                    display.Sides = 4;
                    display.Color = GetHitTypeColor(type);
                    break;
                case HitTypes.Punch:
                    display.Sides = 3;
                    display.Color = GetHitTypeColor(type);
                    display.Angle = 1.5f;
                    break;
                case HitTypes.Kick:
                    display.Sides = 3;
                    display.Color = GetHitTypeColor(type);
                    display.Angle = 0.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private Color GetHitTypeColor(HitTypes hitType)
        {
            switch (hitType)
            {
                case HitTypes.Any:
                    return Color.green;
                case HitTypes.Punch:
                    return Color.yellow;
                case HitTypes.Kick:
                    return Color.blue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hitType), hitType, null);
            }
        }

        public void TieInCombat()
        {
            if(!IsDead && _fsm.CurrentStateID != StateID.TiedInCombat) _fsm.PerformTransition(Transition.ToTiedInCombat);
        }

        public StateID State => _fsm.CurrentStateID;

        private bool IncomingHitCorrect(HitTypes hitType)
        {
            return _hitOrder[CurrentHitTypeIndex] == HitTypes.Any ||
                   _hitOrder[CurrentHitTypeIndex] == hitType;
        }
    }
}