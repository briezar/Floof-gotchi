using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Floof.GameFlowStates;
using UnityEngine;

namespace Floof
{
    public class GameFlowStateMachine : StateMachine
    {
        private InitState _initState;
        private ClassicGameState _classicGameState;

        private MonoBehaviour _coroutineRunner;
        private Coroutine _updateRoutine;

        public GameFlowStateMachine(MonoBehaviour coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;

            _initState = new InitState(this);
            _classicGameState = new ClassicGameState(this);

            _initState.AddStatesToTransit(_classicGameState);

            OnStateChange += CheckUpdate;

            ChangeState(_initState);
        }

        private void CheckUpdate(State newState)
        {
            var type = newState.GetType();
            var bindings = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            var methodInfo = type.GetMethod("OnUpdate", bindings);

            var isUpdateMethodOverridden = methodInfo != null && methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;

            if (!isUpdateMethodOverridden)
            {
                if (_updateRoutine != null) { _coroutineRunner.StopCoroutine(_updateRoutine); }
                return;
            }

            _updateRoutine = _coroutineRunner.StartCoroutine(UpdateRoutine());

            IEnumerator UpdateRoutine()
            {
                while (true)
                {
                    yield return null;
                    StateUpdate();
                }
            }
        }
    }
}