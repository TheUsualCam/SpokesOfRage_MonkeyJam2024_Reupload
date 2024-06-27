using Assets._Scripts.Core_Scripts.StateMachine;
using UnityEngine;

namespace MonkeyJam._Scripts.Core_Scripts.StateMachine
{
    /// <summary>
    /// Core is the core of all the entities, make sure to inherit from the Core class.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Core : MonoBehaviour
    {
        // Core components for the Entity
        [Header("Core entity components")] 
        public Rigidbody2D Body;
        public Animator Animator;
        public SpriteRenderer Sprite;

        // State Machine
        public Machine Machine;
        public State State => Machine.CurrentState;

        /// <summary>
        /// Sets up the instances of the entity.
        /// </summary>
        public void SetUpInstances()
        {
            // instantiate the new state machine
            Machine = new Machine();

            // Get the states from the children GOs
            State[] childStates = GetComponentsInChildren<State>();

            // Iterate through each child and set their core
            foreach (var state in childStates)
            {
                state.SetCore(this);
            }
        }

        /// <summary>
        /// Sets the machine's new state.
        /// </summary>
        /// <param name="state">The new state to set.</param>
        /// <param name="forceReset">Forcefully resets the state.</param>
        public void Set(State state, bool forceReset = false)
        {
            Machine.Set(state, forceReset);
        }
    }
}
