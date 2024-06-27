using System.Collections;
using Assets._Scripts.Core_Scripts.StateMachine;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.Behaviors
{
    public class FlingItem : State
    {
        [Header("Behaviors")] 
        [SerializeField] private IdleState _idle;
        public EntityData Data;
        
        [Header("Animation Properties")] 
        [SerializeField] private AnimationClip _anim;
        [Range(0f, 10f), SerializeField] private float _animSpeed = 1f;
        public float InitialSpeed = 1f;
        [Range(0f, 10f), SerializeField] private float _throwTimer = 3f;

        [Header("Flinging Properties")]
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private LayerMask _targetLayer;
        [Range(1f, 25f), SerializeField] private float _radius;
        [Range(1f, 25f), SerializeField] private float _force;

        [Space] 
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _throwPosition;

        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _clip;

        private bool _stopUpdatingSpeed;

        public bool HasTarget { get; private set; }
        private bool _isRoutineRunning = false;

        private void SetAnimSpeed()
        {
            if (!_stopUpdatingSpeed)
                Animator.speed = _animSpeed;
        }

        public void FindTarget()
        {
            Collider2D[] hitCols = Physics2D.OverlapCircleAll(transform.position, _radius, _targetLayer);
            _target = hitCols.Length > 0 ? hitCols[0].transform : null;

            if (_target != null)
                HasTarget = true;
        }

        private void FaceTarget()
        {
            if (_target != null)
            {
                Vector3 scale = _parent.localScale;
                scale.x = _target.position.x < transform.position.x ? 1 : -1;
                _parent.localScale = scale;
            }
        }

        private IEnumerator FlingItemAtTarget()
        {
            _isRoutineRunning = true;

            while (_target != null)
            {
                _stopUpdatingSpeed = true;

                Animator.speed = InitialSpeed;
                Machine.Set(_idle, true);
                yield return new WaitForSeconds(_throwTimer);

                _stopUpdatingSpeed = false;

                // Check if the target still exists
                if (_target == null)
                {
                    Animator.speed = InitialSpeed;
                    _isRoutineRunning = false;
                    yield break;
                }

                if (Data.IsEntityDead())
                    yield break;

                GameObject go = Instantiate(_itemPrefab, _throwPosition.position, Quaternion.identity);
                Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                
                if (rb == null)
                {
                    rb = go.AddComponent<Rigidbody2D>();
                }

                // Check if the target is still there
                if (_target == null)
                {
                    _isRoutineRunning = false;
                    yield break;
                }

                if (Data.IsEntityDead())
                    yield break;

                // Play the animation of throwing
                Animator.Play(_anim.name);

                Vector3 direction = (_target.position - transform.position).normalized;
                rb.AddForce(direction * _force, ForceMode2D.Impulse);
                _source.clip = _clip;

                if (!_source.isPlaying)
                {
                    _source.Play();
                }

                yield return new WaitForSeconds(_animSpeed);
                Machine.Set(_idle, true);
            }

            _isRoutineRunning = false;
        }

        private void EnemyBehaviour()
        {
            if (_target == null)
            {
                HasTarget = false;
                Set(_idle, true);
            }

            FindTarget();
            FaceTarget();

            if (_target != null && !_isRoutineRunning)
            {
                StartCoroutine(nameof(FlingItemAtTarget));
            }
        }

        #region State Methods

        public override void Enter()
        {
            SetAnimSpeed();
        }

        public override void Do()
        {
            EnemyBehaviour();
            IsComplete = true;
        }

        public override void Exit()
        {
            Animator.speed = InitialSpeed;
        }

        #endregion

        #if UNITY_EDITOR

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        #endif
    }
}
