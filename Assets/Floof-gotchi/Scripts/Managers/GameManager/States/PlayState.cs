using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Dre0Dru.AddressableAssets.Loaders;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Floof.GameFlowStates
{
    public class PlayState : State
    {
        public PlayState(StateMachine stateMachine) : base(stateMachine) { }

        private ScriptableObjectReference<ClassicGameAsset> _classicGameAssetRef;

        protected override async void OnEnter()
        {
            var playViewRef = ViewManager.GetPrefabReference<PlayView>();
            var viewTask = AssetManager.PrefabLoader.LoadAssetAsync(playViewRef);

            var gameAsset = await Addressables.LoadAssetAsync<ClassicGameAsset>(nameof(ClassicGameAsset)).ToUniTask();

            await gameAsset.LoadAll();
            await viewTask;

            var fadeSetting = new FadeSetting(0.5f, 0.5f);
            ViewManager.FadeTransition(fadeSetting);

            var playView = ViewManager.Show<PlayView>();
            ViewManager.Hide<LoadingView>();

            var gamePresenter = GameObject.Instantiate(gameAsset.GetClassicGamePresenter());
            var floofPresenter = GameObject.Instantiate(gameAsset.GetFloofPresenter());

            gamePresenter.Setup(playView, floofPresenter);

            // _gamePresenter = new ClassicGamePresenter(playView);
        }

    }
}
