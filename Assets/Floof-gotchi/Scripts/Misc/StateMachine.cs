using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private HashSet<State> _states = new();

    public State CurrentState { get; private set; }

    public void AddState(State state)
    {
        _states.Add(state);
    }

    public void GotoState(State state, object data = null)
    {
        if (_states.Contains(state))
        {
            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState.OnEnter(data);
        }
        else
        {
            Debug.LogError($"{state.GetType()} is not added!");
        }
    }


    #region Unity function
    private void Start()
    {
        OnSystemStart();
    }

    private void Update()
    {
        OnSystemUpdate();
        CurrentState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        OnSystemFixedUpdate();
        CurrentState?.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        OnSystemLateUpdate();
        CurrentState?.OnLateUpdate();
    }
    #endregion


    public virtual void OnSystemUpdate() { }
    public virtual void OnSystemFixedUpdate() { }
    public virtual void OnSystemLateUpdate() { }
    public virtual void OnSystemStart() { }
}

public abstract class State
{
    public virtual void OnEnter(object data = null) { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnExit() { }

}