﻿using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace _Scripts.FSM_System
{
    /// <summary>
    /// This class represents the States in the Finite State System.
    /// Each state has a Dictionary with pairs (transition-state) showing
    /// which state the FSM should be if a transition is fired while this state
    /// is the current state.
    /// Method Reason is used to determine which transition should be fired .
    /// Method Act has the code to perform the actions the NPC is supposed do if it's on this state.
    /// </summary>
    public abstract class FSMState
    {
        protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
        protected StateID stateID;
        public StateID ID { get { return stateID; } }
 
        public void AddTransition(Transition trans, StateID id)
        {
            // Check if anyone of the args is invalid
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
                return;
            }
 
            if (id == StateID.NullStateID)
            {
                Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
                return;
            }
 
            // Since this is a Deterministic FSM,
            //   check if the current transition was already inside the map
            if (map.ContainsKey(trans))
            {
                Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() + 
                               "Impossible to assign to another state");
                return;
            }
 
            map.Add(trans, id);
        }
 
        /// <summary>
        /// This method deletes a pair transition-state from this state's map.
        /// If the transition was not inside the state's map, an ERROR message is printed.
        /// </summary>
        public void DeleteTransition(Transition trans)
        {
            // Check for NullTransition
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("FSMState ERROR: NullTransition is not allowed");
                return;
            }
 
            // Check if the pair is inside the map before deleting
            if (map.ContainsKey(trans))
            {
                map.Remove(trans);
                return;
            }
            Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() + 
                           " was not on the state's transition list");
        }
 
        /// <summary>
        /// This method returns the new state the FSM should be if
        ///    this state receives a transition and 
        /// </summary>
        public StateID GetOutputState(Transition trans)
        {
            // Check if the map has this transition
            if (map.ContainsKey(trans))
            {
                return map[trans];
            }
            return StateID.NullStateID;
        }
 
        /// <summary>
        /// This method is used to set up the State condition before entering it.
        /// It is called automatically by the FSMSystem class before assigning it
        /// to the current state.
        /// </summary>
        public virtual void DoBeforeEntering() { }
 
        /// <summary>
        /// This method is used to make anything necessary, as reseting variables
        /// before the FSMSystem changes to another one. It is called automatically
        /// by the FSMSystem before changing to a new state.
        /// </summary>
        public virtual void DoBeforeLeaving() { } 
 
        /// <summary>
        /// This method decides if the state should transition to another on its list
        /// NPC is a reference to the object that is controlled by this class
        /// </summary>
        public abstract void Reason();
 
        /// <summary>
        /// This method controls the behavior of the NPC in the game World.
        /// Every action, movement or communication the NPC does should be placed here
        /// NPC is a reference to the object that is controlled by this class
        /// </summary>
        public abstract void Act();
 
    } // class FSMState
 
 
    /// <summary>
    /// FSMSystem class represents the Finite State Machine class.
    ///  It has a List with the States the NPC has and methods to add,
    ///  delete a state, and to change the current state the Machine is on.
    /// </summary>
    public class FSMSystem
    {
        private List<FSMState> states;
 
        // The only way one can change the state of the FSM is by performing a transition
        // Don't change the CurrentState directly
        private ReactiveProperty<StateID> currentStateID;
        public ReactiveProperty<StateID> CurrentStateIdRx => currentStateID;
        public StateID CurrentStateID { get { return currentStateID.Value; } }
        private FSMState currentState;
        public FSMState CurrentState { get { return currentState; } }
 
        public FSMSystem()
        {
            states = new List<FSMState>();
            currentStateID = new ReactiveProperty<StateID>();
        }
 
        /// <summary>
        /// This method places new states inside the FSM,
        /// or prints an ERROR message if the state was already inside the List.
        /// First state added is also the initial state.
        /// </summary>
        public void AddState(FSMState s)
        {
            // Check for Null reference before deleting
            if (s == null)
            {
                Debug.LogError("FSM ERROR: Null reference is not allowed");
            }
 
            // First State inserted is also the Initial state,
            //   the state the machine is in when the simulation begins
            if (states.Count == 0)
            {
                states.Add(s);
                currentState = s;
                currentStateID.Value = s.ID;
                return;
            }
 
            // Add the state to the List if it's not inside it
            foreach (FSMState state in states)
            {
                if (state.ID == s.ID)
                {
                    Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() + 
                                   " because state has already been added");
                    return;
                }
            }
            states.Add(s);
        }
 
        /// <summary>
        /// This method delete a state from the FSM List if it exists, 
        ///   or prints an ERROR message if the state was not on the List.
        /// </summary>
        public void DeleteState(StateID id)
        {
            // Check for NullState before deleting
            if (id == StateID.NullStateID)
            {
                Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
                return;
            }
 
            // Search the List and delete the state if it's inside it
            foreach (FSMState state in states)
            {
                if (state.ID == id)
                {
                    states.Remove(state);
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
            if (trans == Transition.NullTransition)
            {
                Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
                return;
            }
 
            // Check if the currentState has the transition passed as argument
            StateID id = currentState.GetOutputState(trans);
            if (id == StateID.NullStateID)
            {
                Debug.LogError("FSM ERROR: State " + currentStateID.ToString() +  " does not have a target state " + 
                               " for transition " + trans.ToString());
                return;
            }
 
            // Update the currentStateID and currentState		
            currentStateID.Value = id;
            foreach (FSMState state in states)
            {
                if (state.ID == currentStateID.Value)
                {
                    // Do the post processing of the state before setting the new one
                    currentState.DoBeforeLeaving();
 
                    currentState = state;
 
                    // Reset the state to its desired condition before it can reason or act
                    currentState.DoBeforeEntering();
                    break;
                }
            }
 
        } // PerformTransition()
 
    } //class FSMSystem
}