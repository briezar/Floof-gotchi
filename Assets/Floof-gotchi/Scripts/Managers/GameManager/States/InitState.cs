using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof.GameFlowStates
{
    public class InitState : State
    {
        public InitState(StateMachine stateMachine) : base(stateMachine) { }

        private IState _mainMenuState;

        public void AddStatesToTransit(MainMenuState mainMenuState)
        {
            _mainMenuState = mainMenuState;
        }

        public override void OnEnter()
        {
            DG.Tweening.DOTween.Init();

            Debug.unityLogger.logEnabled = false;
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#endif
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;

            var loadingView = ViewManager.Show<LoadingView>();
            // StartCoroutine(InitRoutine(loadUI));

            // IEnumerator InitRoutine(LoadUI loadUI)
            // {
            //     loadUI.Fill = 0f;
            //     float maxFill = 0f;
            //     UIManager.Instance.PreLoadUIs((percent) =>
            //     {
            //         maxFill = percent;
            //     });

            //     yield return loadUI.canvasGroup.DOFade(1f, 0.75f).ChangeStartValue(0f).SetDelay(0.25f).WaitForCompletion();

            //     while (loadUI.Fill < 0.99f)
            //     {
            //         if (loadUI.Fill < maxFill)
            //         {
            //             loadUI.Fill += Time.deltaTime;
            //         }
            //         yield return null;
            //     }
            //     loadUI.Fill = 1f;

            //     var sequence = DOTween.Sequence()
            //     .AppendCallback(() => new ClassicGame.MainController())
            //     .AppendInterval(0.25f)
            //     .Append(loadUI.canvasGroup.DOFade(0f, 0.75f));

            //     yield return sequence.WaitForCompletion();

            //     UIManager.SetInteractable(true);
            //     UIManager.ReleaseUI(loadUI);
            // }

        }

        public override void OnUpdate()
        {

        }

    }
}
