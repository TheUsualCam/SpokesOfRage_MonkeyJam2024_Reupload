using System.Collections;
using Assets._Scripts.Core_Scripts.StateMachine;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.Behaviors
{
    public class BasicAttackMelee : State
    {
        [Header("Animation Properties")]
        public AnimationClip Anim;
        public float AnimSpeed;

        [Space]
        private AudioSource _source;
        [SerializeField] private AudioClip _clip;

        [Space, Header("Attack properties")] 
        [SerializeField] private Transform _attackPoint;
        [SerializeField, Range(0.1f, 5f)] private float _attackRange;
        [SerializeField] private LayerMask _targetLayer;

        [Space, SerializeField, Range(1, 30)]
        private int _damage;

        private bool _damageDealt;

        private void DealDamage()
        {
            // Deal damage within this time
            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !Animator.IsInTransition(0))
            {
                // Detect enemies in range of attack
                Collider2D[] hitCols = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _targetLayer);

                // Damage the hit colliders
                foreach (var enemy in hitCols)
                {
                    var data = enemy.GetComponentInParent<EntityData>();
                    if (data != null)
                    {
                        data.TakeDamage(_damage);
                        _source.clip = _clip;

                        if (!_source.isPlaying)
                        {
                            _source.Play();
                        }
                    }
                }
            }
        }

        public override void Enter()
        {
            Animator.speed = AnimSpeed;
            Animator.Play(Anim.name);
            _source = GetComponentInParent<AudioSource>();
            IsComplete = false;
            _damageDealt = false;
        }

        public override void Do()
        {
            // Mark the attack as complete once the animation finishes
            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !Animator.IsInTransition(0))
            {
                if (!_damageDealt)
                {
                    DealDamage();
                    _damageDealt = true;
                }

                StartCoroutine(WaitForAttackCooldown());
            }
        }

        public override void Exit()
        {
            IsComplete = false;
            Animator.speed = 1f;
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
