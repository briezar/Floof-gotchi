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
            PreloadViews();

            var fadeSetting = FadeSetting.FadeOut(0.5f);

            ViewManager.FadeTransition(fadeSetting);

            _loadingView = await ViewManager.ShowAsync<LoadingView>();

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

            fadeSetting = new FadeSetting(0.5f, 0.5f);
            fadeSetting.OnFadeInComplete = () => ChangeState(_playState);

            ViewManager.FadeTransition(fadeSetting);
        }

        private void PreloadViews()
        {
            AssetManager.PrefabLoader.PreloadAssetAsync(ViewManager.GetPrefabReference<PlayView>());
        }

    }

}
