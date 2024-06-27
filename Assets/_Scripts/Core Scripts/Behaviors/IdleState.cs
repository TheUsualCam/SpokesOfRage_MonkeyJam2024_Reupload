using Assets._Scripts.Core_Scripts.StateMachine;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.Behaviors
{
    public class IdleState : State
    {
        public AnimationClip Animation;
        public float AnimSpeed;

        public override void Enter()
        {
            Animator.Play(Animation.name);
            Animator.speed = AnimSpeed;
        }

        public override void Do()
        {
            if (!(Body.velocity == Vector2.zero))
            {
                IsComplete = true;
            }
        }

        public override void Exit()
        {
            Animator.speed = 0f;
        }
    }
}
