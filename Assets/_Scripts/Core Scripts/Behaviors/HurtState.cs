using System.Collections;
using Assets._Scripts.Core_Scripts.StateMachine;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.Behaviors
{
    public class HurtState : State
    {
        public AnimationClip Animation;
        [SerializeField] private float _animSpeed = 0.2f;

        [Space] 
        public Transform Entity;
        public Vector2 AppliedForce;

        private AudioSource _source;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private GameObject _comicSprite;

        public override void Enter()
        {
            Animator.speed = _animSpeed;
            Animator.Play(Animation.name);
            _source = GetComponentInParent<AudioSource>();
            _source.clip = _clip;

            if (!_source.isPlaying)
            {
                _source.Play();
            }

            _comicSprite.transform.localScale = new Vector2(-transform.parent.localScale.x, transform.localScale.y);
            _comicSprite.SetActive(true);
            StartCoroutine(WaitForDeactivateComicSprite());

            IsComplete = false;
        }

        public override void FixedDo()
        {
            float dir = Mathf.Sign(Entity.localScale.x);
            Body.AddForce(new Vector2(-dir * AppliedForce.x, AppliedForce.y), ForceMode2D.Force);
        }

        public override void Exit()
        {
            Animator.speed = 1f;
        }

        IEnumerator WaitForDeactivateComicSprite()
        {
            yield return new WaitForSeconds(0.3f);
            _comicSprite.SetActive(false);
        }
    }
}
