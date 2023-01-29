using Dewdrop.Debugging;

namespace Dewdrop.StateMachines
{
    /// <summary>
    /// This is a finite state machine. It can only have a single state at a time.
    /// </summary>
    public class StateMachine
    {
        public State CurrentState;
        public State NextState;

        private readonly State _initialState;

        /// <summary>
        /// The name of the state machine.
        /// </summary>
        public string machineName;

        /// <summary>
        /// Creates a new state machine with the given initial state and machine name
        /// </summary>
        /// <param name="CurrentState">The initial state of the machine. When Initialize() is called, this will be the state that it's set to.</param>
        /// <param name="machineName">The name of the machine</param>
        public StateMachine(State CurrentState, string machineName)
        {
            _initialState = CurrentState;
            this.machineName = machineName;
            this.CurrentState = new EmptyState();
        }

        /// <summary>
        /// Initalizes the state machine.
        /// </summary>
        public void Initialize()
        {
            if (this.NextState != null)
            {
                this.SetState(this.NextState);
                return;
            }

            if (CurrentState is EmptyState && _initialState != null && _initialState.GetType().IsSubclassOf(typeof(State)))
            {
                this.SetState(_initialState);
            }
        }

        /// <summary>
        /// Checks if the state machine has a next state.
        /// </summary>
        /// <returns>If true, the state machine has a next state, if false, it doesn't.</returns>
        public bool HasNextState()
        {
            return NextState != null;
        }

        /// <summary>
        /// Sets the next state of this state machine.
        /// </summary>
        /// <param name="state">The state to set the next state to.</param>
        public void SetNextState(State state)
        {
            NextState = state;
        }

        /// <summary>
        /// Sets the state of the state machine to the specified state.
        /// </summary>
        /// <param name="newState">The new state to set the state machine to.</param>
        public void SetState(State newState)
        {
            if (newState != null)
            {
                NextState = null;
                newState.StateMachine = this;

                CurrentState.OnExit();
                CurrentState = newState;
                CurrentState.OnEnter();
            }
            else
            {
                DBG.LogWarning($"Tried to go into null state on state machine '{machineName}'.");
            }
            //insert network code
        }


        /// <summary>
        /// Before setting the state of the state machine, this method will check EntityState.CanBeInterrupted to see if the current state can be interrupted. 
        /// </summary>
        /// <param name="newState">The new state to set the state machine to.</param>
        public void SetStateInterrupt(State newState)
        {
            if (CurrentState.CanBeInterrupted() && newState != null)
            {
                NextState = null;
                newState.StateMachine = this;

                CurrentState.OnExit();
                CurrentState = newState;
                CurrentState.OnEnter();
            }
            else
            {
                DBG.LogWarning($"Either the new state was null, or the current state on state machine '{machineName}' cannot be interrupted.");
            }
        }

        /// <summary>
        /// Sets the state of this state machine to EmptyState.
        /// </summary>
        public void SetEmpty()
        {
            NextState = new EmptyState();
        }

        /// <summary>
        /// Updates the current state
        /// </summary>
        public void Update()
        {
            if (NextState != null)
            {
                SetState(NextState);
            }
            CurrentState.Update();
        }

        /// <summary>
        /// Acts like a dispose method, calls the OnExit method of the current state and sets it to null.
        /// </summary>
        private void Remove()
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
                CurrentState = null;
                NextState = null;
            }
        }
    }
}