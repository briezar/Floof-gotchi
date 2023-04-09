using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public enum UpdateStyle { Update, FixedUpdate, LateUpdate }

    private HashSet<State> _states = new();

    public State CurrentState { get; private set; }
    private HashSet<UpdateStyle> _updateStyles;

    public void SetUpdateStyles(params UpdateStyle[] updateStyles)
    {
        _updateStyles = new(updateStyles);
    }

    public void AddState(State state)
    {
        if (!_states.Add(state))
        {
            Debug.LogWarning($"{state.GetType()} is already added!");
        };
    }

    public void GoToState(State state, object data = null)
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

    private void Start()
    {
        if (_updateStyles.IsNullOrEmpty()) { return; }

        YieldInstruction yieldInstruction = null;
        foreach (var updateStyle in _updateStyles)
        {
            switch (updateStyle)
            {
                case UpdateStyle.Update:
                    RunUpdate(() =>
                    {
                        OnSystemUpdate();
                        CurrentState?.OnUpdate();
                    });
                    break;

                case UpdateStyle.FixedUpdate:
                    yieldInstruction = new WaitForFixedUpdate();
                    RunUpdate(() =>
                    {
                        OnSystemFixedUpdate();
                        CurrentState?.OnFixedUpdate();
                    });
                    break;

                case UpdateStyle.LateUpdate:
                    yieldInstruction = new WaitForEndOfFrame();
                    RunUpdate(() =>
                    {
                        OnSystemLateUpdate();
                        CurrentState?.OnLateUpdate();
                    });
                    break;
            }
        }

        void RunUpdate(Action onUpdate)
        {
            StartCoroutine(UpdateRoutine());
            IEnumerator UpdateRoutine()
            {
                while (true)
                {
                    yield return yieldInstruction;
                    onUpdate?.Invoke();
                }
            }
        }
    }

    public virtual void OnSystemUpdate() { }
    public virtual void OnSystemFixedUpdate() { }
    public virtual void OnSystemLateUpdate() { }
}

public abstract class State
{
    private StateMachine _stateMachine;

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }


    protected void GoToState(State state, object data = null)
    {
        _stateMachine.GoToState(state, data);
    }

    public virtual void OnEnter(object data = null) { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnExit() { }

}