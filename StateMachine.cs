using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State machine utility system and state interface to easily run states and their assigned functions
/// </summary>

namespace NB
{
    /// <summary>
    /// A simple state machine system
    /// </summary>
    /// <remarks>
    /// Inluded in script are the NBIState interface, NBState class and StateMachine class for easier cross reference
    /// Normally these would be in separate scripts
    /// </remarks>
    public interface NBIState
    {
        void Enter();
        void Execute();
        void Exit();
    }

    /// <summary>
    /// Delegate to assign statemachine callbacks to
    /// </summary>
    public delegate void Callback();

    /// <summary>
    /// The base State class
    /// </summary>
    public class NBState : NBIState
    {
        private Callback enter;

        private Callback execute;

        private Callback exit;

        public string name;

        private bool init = false;

        /// <summary>
        /// State Constructor
        /// </summary>
        /// <param name="enter"> The function to be the enter callback </param>
        /// <param name="execute"> The function to be the execute callback </param>
        /// <param name="exit"> The function to be the exit callback </param>
        /// <param name="name"> The name of the state </param>
        public NBState(Callback enter, Callback execute, Callback exit, string name)
        {
            this.enter = enter;

            this.execute = execute;

            this.exit = exit;

            this.name = name;

            init = true;
        }

        public void Enter()
        {
            if (init)
            {
                if (enter != null)
                {
                    enter();
                }
            }
        }

        public void Execute()
        {
            if (init)
            {
                if (execute != null)
                {
                    execute();
                }
            }
        }

        public void Exit()
        {
            if (init)
            {
                if (exit != null)
                {
                    exit();
                }
            }
        }
    }

    /// <summary>
    /// The base State Machine class
    /// </summary>
    public class StateMachine
    {
        private NBState currentState;

        private List<NBState> states;

        private string name;

        private bool running = false;

        /// <summary>
        /// State Machine Constructor
        /// </summary>
        /// <param name="name"> The name of the state </param>
        public StateMachine(string name)
        {
            states = new List<NBState>();

            this.name = name;
        }

        /// <summary>
        /// State Machine Constructor that takes in a state
        /// </summary>
        /// <param name="name"> The name of the state </param>
        /// <param name="newState"> The state to be added to the list </param>
        public StateMachine(string name, NBState newState)
        {
            states = new List<NBState>();

            states.Add(newState);

            this.name = name;
        }

        /// <summary>
        /// State Machine Constructor that takes in a list of states
        /// </summary>
        /// <param name="name"> The name of the state machine </param>
        /// <param name="states"> The list of states for the state machine </param>
        public StateMachine(string name, List<NBState> states)
        {
            this.states = new List<NBState>(states);

            this.name = name;
        }

        /// <summary>
        /// Adds a state to the list of states if not already added
        /// </summary>
        /// <param name="newState"> The new state to be added to the list </param>
        public void AddState(NBState newState)
        {
            for(int i = 0; i < states.Count; i++)
            {
                if (states[i].name.Equals(newState.name))
                {
                    Debug.LogError("State already exists: " + newState.name);
                }
            }

            states.Add(newState);
        }

        /// <summary>
        /// Finds the name of the current state
        /// </summary>
        /// <returns> The name of the current state, or null if not found </returns>
        public string GetCurrentState()
        {
            if (running)
            {
                return currentState.name;
            }

            return "null";
        }

        /// <summary>
        /// Goes to the state given by name
        /// </summary>
        /// <param name="stateName"> The name of the state to transition to </param>
        public void GoToState(string stateName)
        {
            if(currentState == null)
            {
                for (int i = 0; i < states.Count; i++)
                {
                    if (states[i].name.Equals(stateName))
                    {
                        currentState = states[i];
                    }
                }

                if(currentState == null)
                {
                    Debug.LogError("Could not start state machine " + name + ", state " + stateName + " not found in list of states");
                }
                else
                {
                    running = true;

                    currentState.Enter();
                }
            }
            else
            {
                currentState.Exit();

                string oldStateName = currentState.name;

                for(int i = 0; i < states.Count; i++)
                {
                    if (states[i].name.Equals(stateName))
                    {
                        currentState = states[i];
                    }
                }

                if (currentState.name.Equals(oldStateName))
                {
                    Debug.LogError("Could not go to state " + stateName + ", not found in list of states");
                }
                else
                {
                    currentState.Enter();
                }
            }
        }

        /// <summary>
        /// The update function for the State Machine, must be called in a monobehavior class
        /// </summary>
        public void StateMachineUpdate()
        {
            if (running)
            {
                currentState.Execute();
            }
        }
    }
}
