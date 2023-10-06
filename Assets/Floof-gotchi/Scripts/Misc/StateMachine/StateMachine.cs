using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }

    public abstract class State : IState
    {
        private readonly StateMachine _stateMachine;

        private State() { }
        public State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        protected void ChangeState(IState state)
        {
            _stateMachine.ChangeState(state);
        }

        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }

        void IState.OnEnter() => OnEnter();
        void IState.OnUpdate() => OnUpdate();
        void IState.OnExit() => OnExit();
    }

    public class StateMachine
    {
        public event Action<State> OnStateChange;
        private IState _currentState;

        public void StateUpdate()
        {
            if (_currentState == null) { return; }

            _currentState.OnUpdate();
        }

        public void ChangeState(IState state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _currentState.OnEnter();

            OnStateChange?.Invoke(state as State);
        }

        public void Shutdown()
        {
            _currentState.OnExit();
            _currentState = null;
        }
    }
}