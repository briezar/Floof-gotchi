using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public interface IState { }

    public abstract class State : IState
    {
        private StateMachine _stateMachine;

        private State() { }
        public State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        protected void ChangeState(IState state)
        {
            _stateMachine.ChangeState(state);
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }

    public class StateMachine
    {
        private State _currentState;

        public void StateUpdate()
        {
            if (_currentState == null) { return; }

            _currentState.OnUpdate();

        }

        public void ChangeState(IState state)
        {
            _currentState?.OnExit();
            _currentState = state as State;
            _currentState.OnEnter();
        }

        public void Shutdown()
        {
            _currentState.OnExit();
            _currentState = null;
        }
    }
}