using System.Collections;
using System.Collections.Generic;
using Floof.GameFlowStates;
using UnityEngine;

namespace Floof
{
    public class GameFlowStateMachine : StateMachine
    {
        private InitState _initState;
        private MainMenuState _mainMenuState;
        public GameFlowStateMachine()
        {
            _initState = new InitState(this);
            _mainMenuState = new MainMenuState(this);

            _initState.AddStatesToTransit(_mainMenuState);
            ChangeState(_initState);
        }
    }
}