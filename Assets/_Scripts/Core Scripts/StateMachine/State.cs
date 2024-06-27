using MonkeyJam._Scripts.Core_Scripts.StateMachine;
using UnityEngine;

namespace Assets._Scripts.Core_Scripts.StateMachine
{
    public class State : MonoBehaviour
    {
        public bool IsComplete { get; protected set; }

        protected float StartTime;
        public float RunTime => Time.time - StartTime;

        // blackboard vars
        protected Core Core;
        protected Rigidbody2D Body => Core.Body;
        protected Animator Animator => Core.Animator;

        // State machine
        public Machine Machine;
        public Machine Parent;

        public State CurrentState => Machine.CurrentState;

        /// <summary>
        /// Sets the State Machine's new state.
        /// </summary>
        /// <param name="newState">The new state to set active.</param>
        /// <param name="forceReset">Determines if the state should stop or not.</param>
        protected void Set(State newState, bool forceReset = false)
        {
            Machine.Set(newState, forceReset);
        }

        /// <summary>
        /// Initialises a new core and also a new state machine.
        /// </summary>
        /// <param name="core">The core to set.</param>
        public void SetCore(Core core)
        {
            Machine = new();
            Core = core;
        }

        public virtual void Enter() { }
        public virtual void Do() { }
        public virtual void FixedDo() { }
        public virtual void Exit() { }

        public void DoBranch()
        {
            Do();
            CurrentState?.DoBranch();
        }

        public void FixedDoBranch()
        {
            FixedDo();
            CurrentState?.FixedDoBranch();
        }

        public virtual void Initialise(Machine parent)
        {
            Parent = parent;
            IsComplete = false;

            StartTime = Time.time;
        }
    }
}
