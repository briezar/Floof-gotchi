using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof.GameFlowStates
{
    public class PlayState : State
    {
        public PlayState(StateMachine stateMachine) : base(stateMachine) { }

        private ClassicGameController _gameController;

        protected override void OnEnter()
        {
            var playView = ViewManager.Show<PlayView>();
            ViewManager.Hide<LoadingView>();

            _gameController = new ClassicGameController(playView);
        }

    }
}
