using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// StateSystem class represents the Finite State Machine class.
///  It has a List with the States the NPC has and methods to add,
///  delete a state, and to change the current state the Machine is on.
/// </summary>
public class StateSystem 
{
    private List<State> _states;

    private StateID _currentStateID;
    public StateID CurrentStateID
    {
        get { return _currentStateID; }
    }

    private State _currentState;
    public State CurrentState
    {
        get { return _currentState; }
    }

    private StateID _lastStateID;
    public StateID LastStateID
    {
        get { return _lastStateID; }
    }

    public StateSystem()
    {
        _states = new List<State>();
    }

    /// <summary>
    /// This method places new states inside the FSM,
    /// or prints an ERROR message if the state was already inside the List.
    /// First state added is also the initial state.
    /// </summary>
    public void AddState(State s)
    {
        // Check for Null reference before deleting
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
        if (_states.Count == 0)
        {
            _states.Add(s);
            _currentState = s;
            _lastStateID = StateID.eStateID_Null;
            _currentStateID = s.ID;
            _currentState.OnEnter();
            return;
        }

        // Add the state to the List if it's not inside it
        foreach (State state in _states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
                return;
            }
        }
        _states.Add(s);
    }

      /// <summary>
    /// This method delete a state from the FSM List if it exists, 
    ///   or prints an ERROR message if the state was not on the List.
    /// </summary>
    public void DeleteState(StateID id)
    {
        // Check for NullState before deleting
        if (id == StateID.eStateID_Null)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        // Search the List and delete the state if it's inside it
        foreach (State state in _states)
        {
            if (state.ID == id)
            {
                _states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    /// <summary>
    /// This method tries to change the state the FSM is in based on
    /// the current state and the transition passed. If current state
    ///  doesn't have a target state for the transition passed, 
    /// an ERROR message is printed.
    /// </summary>
    public void PerformTransition(Transition trans)
    {
        // Check for NullTransition before changing the current state
        if (trans == Transition.eTransiton_Null)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

         // Check if the currentState has the transition passed as argument
        StateID id = _currentState.GetOutputState(trans);
        if (id == StateID.eStateID_Null)
        {
            Debug.LogWarning("FSM ERROR: State " + _currentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }
        // Update the currentStateID and currentState		
        _currentStateID = id;
        foreach (State state in _states)
        {
            if (state.ID == _currentStateID)
            {
                // Do the post processing of the state before setting the new one
                _currentState.OnExit();
                _lastStateID = _currentState.ID;
                _currentState = state;

                // Reset the state to its desired condition before it can reason or act
                _currentState.OnEnter();
                break;
            }
        }
    } // PerformTransition()

    public State GetState(StateID id)
    {
        // Check for NullState before deleting
        if (id == StateID.eStateID_Null)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return null;
        }

        // Search the List and delete the state if it's inside it
        foreach (State state in _states)
        {
            if (state.ID == id)
            {
                return state;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to get state " + id.ToString() +
                       ". It was not on the list of states");
        return null;
    }
} //class StateSystem
