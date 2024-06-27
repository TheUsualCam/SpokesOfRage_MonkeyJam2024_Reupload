using System.Collections;
using Assets._Scripts.Core_Scripts.StateMachine;
using MonkeyJam._Scripts.Core_Scripts.Entities;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.Behaviors
{
    public class DeathState : State
    {
        public AnimationClip Animation;
        private AudioSource _source;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private GameObject _comicSprite;
        [SerializeField] private float _timeUntilDestruction;
        [SerializeField] private bool _destroyObject = false;

        private bool _playedDeathSound = false;
        private bool _playedDeathEffect = false;

        public override void Enter()
        {
            Animator.Play(Animation.name);
            _source = GetComponentInParent<AudioSource>();
            _source.clip = _clip;
            if (!_playedDeathSound)
            {
                _playedDeathSound = true;
                _source.Play();
            }
            Animator.Play(Animation.name);

            if (!_playedDeathEffect)
            {
                _comicSprite.transform.localScale = new Vector2(-transform.parent.localScale.x, transform.localScale.y);
                _comicSprite.SetActive(true);
                StartCoroutine(WaitForDeactivateSprite());
            }

            if (_destroyObject)
                StartCoroutine(DeleteObject());

            IsComplete = false;
        }

        IEnumerator DeleteObject()
        {
            GameObject go = GetComponentInParent<EntityData>().gameObject;
            yield return new WaitForSeconds(_timeUntilDestruction);
            Destroy(go);
        }

        IEnumerator WaitForDeactivateSprite()
        {
            yield return new WaitForSeconds(0.3f);
            _playedDeathEffect = true;
            _comicSprite.SetActive(false);
        }
    }
}
