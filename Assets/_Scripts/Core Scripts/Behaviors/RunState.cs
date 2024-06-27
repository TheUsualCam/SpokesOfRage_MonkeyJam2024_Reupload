using Assets._Scripts.Core_Scripts;
using Assets._Scripts.Core_Scripts.StateMachine;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.Behaviors
{
    public class RunState : State
    {
        public AnimationClip Anim;
        public float AnimSpeed = 1f;
        private float _defaultMappedValue = 1f;
        public bool UseVelocity = false;

        public override void Enter()
        {
            Animator.Play(Anim.name);
        }

        public override void Do()
        {
            float mappedSpeed = 0;
            if (UseVelocity)
            {
                // Change mapped speed based on velocity
                _defaultMappedValue = Mathf.Abs(Body.velocity.x) + Mathf.Abs(Body.velocity.y);
                mappedSpeed = Helpers.Map(_defaultMappedValue, 0, 1, 0, AnimSpeed, true);
            }
            else
            {
                mappedSpeed = Helpers.Map(_defaultMappedValue, 0, 1, 0, AnimSpeed, true);
            }
            
            // Set the animator speed
            Animator.speed = mappedSpeed;

            // check if there is no velocity anymore, if so end the state.
            if (Body.velocity == Vector2.zero)
                IsComplete = true;
        }

        public override void Exit()
        {
            Animator.speed = 1f;
        }
    }
}
