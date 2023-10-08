using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Floof.GameFlowStates
{
    public class PlayState : State
    {
        public PlayState(StateMachine stateMachine) : base(stateMachine) { }

        private ClassicGameAsset _classicGameAsset;
        private bool _isPreloading;

        public async UniTask Preload()
        {
            if (_isPreloading)
            {
                await UniTask.WaitUntil(() => !_isPreloading);
                return;
            }

            _isPreloading = true;

            var viewTask = ViewManager.Preload<PlayView>();

            var address = new Address(nameof(ClassicGameAsset), nameof(ScriptableObject));

            _classicGameAsset = await AssetManager.ScriptableObjectLoader.LoadAssetAsync(address) as ClassicGameAsset;

            await _classicGameAsset.LoadAll();

            await viewTask;

            _isPreloading = false;
        }

        protected override async void OnEnter()
        {
            await Preload();

            var fadeSetting = new FadeSetting(0.5f, 0.5f);
            fadeSetting.OnFadeInComplete = () =>
            {
                var playView = ViewManager.Show<PlayView>();
                ViewManager.Hide<LoadingView>(true);

                var gamePresenter = GameObject.Instantiate(_classicGameAsset.GetClassicGamePresenter(), GameManager.GameContainer);
                var floofPresenter = GameObject.Instantiate(_classicGameAsset.GetFloofPresenter());

                gamePresenter.Setup(playView, floofPresenter);
            };

            ViewManager.FadeTransition(fadeSetting);

        }

    }
}
