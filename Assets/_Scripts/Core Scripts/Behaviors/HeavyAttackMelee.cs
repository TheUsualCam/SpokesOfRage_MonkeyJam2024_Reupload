using System.Collections;
using Assets._Scripts.Core_Scripts.StateMachine;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.Behaviors
{
    public class HeavyAttackMelee : State
    {
        [Header("Animation Properties")]
        public AnimationClip Anim;
        public float AnimSpeed;

        [Header("Attack properties")]
        [SerializeField] private Transform _attackPoint;
        [SerializeField, Range(0.1f, 5f)] private float _attackRange;
        [SerializeField] private LayerMask _targetLayer;

        [Space, SerializeField, Range(1, 30)]
        private int _damage;

        private AudioSource _source;
        [SerializeField] private AudioClip _clip;

        private bool _dealtDamage;

        private void DealDamage()
        {
            // End the state once the animation ends, handle all damage etc. in here
            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !Animator.IsInTransition(0))
            {
                // Detect enemies in range of attack
                Collider2D[] hitCols = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _targetLayer);

                // Damage the hit colliders
                foreach (var enemy in hitCols)
                {
                    var data = enemy.GetComponentInParent<EntityData>();
                    if (data != null)
                        data.TakeDamage(_damage);
                    _source.clip = _clip;

                    if (!_source.isPlaying)
                    {
                        _source.Play();
                    }
                }

                IsComplete = true;
            }
        }

        public override void Enter()
        {
            Animator.Play(Anim.name);
            _source = GetComponentInParent<AudioSource>();
            IsComplete = false;
            _dealtDamage = false;
        }

        public override void Do()
        {
            Animator.speed = AnimSpeed;

            // Mark the attack as complete once the animation finishes
            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !Animator.IsInTransition(0))
            {
                if (!_dealtDamage)
                {
                    DealDamage();
                    _dealtDamage = true;
                }

                StartCoroutine(WaitForAttackCooldown());
            }
        }

        IEnumerator WaitForAttackCooldown()
        {
            yield return new WaitForSeconds(0.2f);
            IsComplete = true;
        }

#if UNITY_EDITOR

        void OnDrawGizmos()
        {
            if (_attackPoint == null || !_attackPoint.gameObject.activeSelf)
                return;

            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
        }

#endif
    }
}