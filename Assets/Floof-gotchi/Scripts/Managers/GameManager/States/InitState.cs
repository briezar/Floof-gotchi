using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Floof.GameFlowStates
{
    public class InitState : State
    {
        public InitState(StateMachine stateMachine) : base(stateMachine) { }

        private PlayState _playState;

        private LoadingView _loadingView;
        private bool _finishedLoading;

        public void AddStatesToTransit(PlayState playState)
        {
            _playState = playState;
        }

        protected override async void OnEnter()
        {
            var loadingViewRef = ViewManager.GetPrefabReference<LoadingView>();

            var preloadTask = AssetManager.PrefabLoader.PreloadAssetsAsync(loadingViewRef);

            var fadeSetting = FadeSetting.FadeOut(0.5f);

            await UniTask.WaitUntil(() => AssetManager.PrefabLoader.IsAssetLoaded(loadingViewRef));

            ViewManager.FadeTransition(fadeSetting);

            _loadingView = ViewManager.Show<LoadingView>();

            _loadingView.Fill = 0;

            await UniTask.WaitForSeconds(0.75f);

            var elapsedTime = 0f;
            var minWaitTime = 1f;
            var max = 0.8f;

            while (!_finishedLoading)
            {
                elapsedTime += Time.deltaTime;
                _loadingView.Fill = Mathf.Lerp(0f, max, elapsedTime / minWaitTime);
                if (elapsedTime > minWaitTime) { _finishedLoading = true; }
                await UniTask.NextFrame();
            }

            while (_loadingView.Fill < 1)
            {
                _loadingView.Fill += Time.deltaTime;
                await UniTask.NextFrame();
            }

            _loadingView.SetText("Complete!");

            await UniTask.WaitForSeconds(0.25f);

            ChangeState(_playState);
        }

    }

}
