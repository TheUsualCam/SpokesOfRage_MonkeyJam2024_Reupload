namespace Assets._Scripts.Core_Scripts.StateMachine
{
    /// <summary>
    /// Machine is the StateMachine.
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// The current state the entity is in.
        /// </summary>
        public State CurrentState;

        /// <summary>
        /// Sets a new state.
        /// </summary>
        /// <param name="newState">The new state to set.</param>
        /// <param name="forceReset">Forces to reset the state.</param>
        public void Set(State newState, bool forceReset = false)
        {
            // Check if the new state is not the old state
            if (CurrentState == newState && !forceReset) return;
            
            // Exit, Enter and initialise the state
            CurrentState?.Exit();
            CurrentState = newState;
            
            // initialize and enter the news tate
            CurrentState.Initialise(this);
            CurrentState.Enter();
        }
    }
}
