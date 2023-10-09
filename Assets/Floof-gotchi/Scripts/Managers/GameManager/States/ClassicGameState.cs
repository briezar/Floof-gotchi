using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Floof.GameFlowStates
{
    public class ClassicGameState : State
    {
        public ClassicGameState(StateMachine stateMachine) : base(stateMachine) { }

        private PrefabReference<ClassicGamePresenter> _gamePresenterRef;
        private bool _isLoaded;

        public async UniTask Preload()
        {
            if (_isLoaded) { return; }

            var viewTask = ViewManager.Preload<PlayView>();

            var address = new Address(nameof(ClassicGamePresenter));
            var location = AssetManager.GetLocations(address, typeof(GameObject))[0];

            _gamePresenterRef = new PrefabReference<ClassicGamePresenter>(location.PrimaryKey);

            await AssetManager.PrefabLoader.LoadAssetAsync(_gamePresenterRef);

            await viewTask;

            _isLoaded = true;
        }

        protected override void OnEnter()
        {
            var playView = ViewManager.Show<PlayView>();

            var gamePresenter = GameObject.Instantiate(_gamePresenterRef.Component, GameManager.GameContainer);
            gamePresenter.Init(playView);

            ViewManager.Hide<LoadingView>(true);
        }

    }
}
