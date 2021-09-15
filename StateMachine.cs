using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State machine utility class and state interface to easily run states and their functions
/// </summary>
/// <remarks>
/// Best utilised at the beginning of a project, can replace IEnumerator usage and manage game states
/// - interface has enter, execute and exit for each state that can be accessed by all classes that inherit from the state machine class
/// - state machine should be created and utilised through another script, refer to StateMachineExample script
/// - can be expanded and customised, this really is the basic structure
/// </remarks>

namespace NB
{
    /// <summary>
    /// A simple state interface
    /// </summary>
    /// <remarks>
    /// Improvements: create wrapper class on the interface which allows greater control over the three functions
    /// : use delegates in place of functions to allow existing functions in other classes to be used as the enter/execute/exit functions
    /// </remarks>
    public interface NBIState
    {
        void Enter();
        void Execute();
        void Exit();
    }

    /// <summary>
    /// A simple state machine class
    /// </summary>
    /// <remarks>
    /// Improvements: keep a list of states rather than load each state in as new, and hold the current state in a separate variable
    /// : give the state machine (and its states) its own name when you intend to have multiple state machines (which you will) to keep track of each 
    /// : allow adding, removing, finding current or any states by name in the list stored in the state machine
    /// </remarks>
    public class StateMachine
    {
        NBIState currentState;

        public void ChangeState(NBIState newState)
        {
            if (currentState != null)
                currentState.Exit();

            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            if (currentState != null)
                currentState.Execute();
        }
    }
}
