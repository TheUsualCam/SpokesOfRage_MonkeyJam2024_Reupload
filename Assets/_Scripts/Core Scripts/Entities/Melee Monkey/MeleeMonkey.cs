using System.Collections;
using Assets._Scripts.Core_Scripts.StateMachine;
using MonkeyJam._Scripts.Core_Scripts.Behaviors;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using MonkeyJam._Scripts.Core_Scripts.StateMachine;
using UnityEngine;

namespace MonkeyJam.Assets._Scripts.Core_Scripts.Entities.Melee_Monkey
{
    public class MeleeMonkey : Core
    {
        [Header("Data")]
        public EntityData Data;

        [Header("Behaviors")]
        public BasicAttackMelee BasicAttack;
        public IdleState Idle;
        public RunState Run;
        public HurtState Hurt;
        public DeathState Death;

        [Space]
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _attackCooldown = 0.3f;
        [SerializeField] private float _stunCooldown;

        [Header("AI Parameters")]
        [SerializeField] private Transform _target;

        [Space]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _detectionRadius;
        [SerializeField] private float _maintainDistance = 1f;

        private bool _canAttack = true;
        private bool _isAttacking;

        void Start()
        {
            Data = GetComponent<EntityData>();

            // Set up the instances of the Entity
            SetUpInstances();

            // Set machine state.
            Machine = new Machine();
            Machine.Set(Idle);
        }

        private void Update()
        {
            if (Data.IsEntityDead())
            {
                Machine.Set(Death);
                return;
            }

            if (_target == null || Time.frameCount % 20 == 0)
                FindTarget();

            _canAttack = TargetInRange() && !_isAttacking;

            if (_target != null)
                FaceTarget();

            SelectState();
            Machine.CurrentState.Do();
        }

        private void FixedUpdate()
        {
            if (Data.IsEntityDead()) return;

            if (!Data.IsTakingDamage)
            {
                HandleMovement();
            }

            Machine.CurrentState.FixedDo();
        }

        private void SelectState()
        {
            if (Data.IsTakingDamage)
            {
                Machine.Set(Hurt);
                StartCoroutine(WaitForStunCooldown());
                return;
            }

            if (_canAttack && _target != null)
            {
                StartBasicAttack();
                return;
            }

            if (_target != null && TargetInDetectionRange())
            {
                if (!Data.IsTakingDamage && !_isAttacking)
                    Machine.Set(Run);
                else
                    Machine.Set(Idle);
            }
        }

        private void StartBasicAttack()
        {
            _isAttacking = true;
            Machine.Set(BasicAttack, true);
            StartCoroutine(CompleteAttack());
        }

        private IEnumerator CompleteAttack() {
            _canAttack = false;
            yield return new WaitForSeconds(_attackCooldown);
            _isAttacking = false;
            _canAttack = true;
        }

        private void HandleMovement()
        {
            if (_target == null || _isAttacking) return;

            if (Vector3.Distance(transform.position, _target.position) > _maintainDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, _target.position, _moveSpeed * Time.deltaTime);
            }
        }

        private void FindTarget()
        {
            Collider2D[] playerCollider = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _targetLayer);
            _target = playerCollider.Length > 0 ? playerCollider[0].transform : null;
        }

        private void FaceTarget()
        {
            if (_target != null)
            {
                Vector3 scale = transform.localScale;
                scale.x = _target.position.x < transform.position.x ? -1 : 1;
                transform.localScale = scale;
            }
        }

        private bool TargetInRange()
        {
            return _target != null && Vector3.Distance(transform.position, _target.position) <= _attackRadius;
        }

        private bool TargetInDetectionRange()
        {
            return _target != null && Vector3.Distance(transform.position, _target.position) <= _detectionRadius;
        }

        IEnumerator WaitForStunCooldown()
        {
            yield return new WaitForSeconds(_stunCooldown);
            Data.ResetIsTakingDamage();
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRadius);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
#endif
    }
}
