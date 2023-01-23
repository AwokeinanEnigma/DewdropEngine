using System;

namespace Dewdrop.StateMachines
{
    /// <summary>
    /// An EntityState is the state part of the finite-state machine model.
    /// </summary>
    public class State
    {
        /// <summary>
        /// The state machine that is executing this state.
        /// </summary>
        public StateMachine stateMachine;


        /// <summary>
        /// This method is called when the state is first executed by a state machine.
        /// </summary>
        public virtual void OnEnter() { }


        /// <summary>
        /// This method is called when the state machine is transitioning over to another state. This is the place to clean up
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// This method is called every frame.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// This is used by the state machine to see if this state can be interrupted.
        /// </summary>
        /// <returns>If true, then this state can be interrupted. If false, it cannot.</returns>
        public virtual bool CanBeInterrupted()
        {
            return true;
        }
    }

}
