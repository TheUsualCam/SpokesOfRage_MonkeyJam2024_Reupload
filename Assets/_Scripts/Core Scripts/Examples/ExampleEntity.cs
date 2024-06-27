using Assets._Scripts.Core_Scripts.StateMachine;
using MonkeyJam._Scripts.Core_Scripts.Behaviors;
using MonkeyJam._Scripts.Core_Scripts.StateMachine;
using UnityEngine;

namespace Assets._Scripts.Core_Scripts.Examples
{
    public class ExampleEntity : Core
    {
        [Header("Behaviors")]
        public RunState RunState;

        void Start()
        {
            // get the body
            Body = GetComponent<Rigidbody2D>();

            // Set up instances
            SetUpInstances();

            // Initialise new state machine
            Machine = new Machine();
            Machine.Set(RunState);
        }

        void Update()
        {
            // Do the branch of the entity
            State.DoBranch();
        }
    }
}
