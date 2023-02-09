using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(Instance.gameObject);
        DontDestroyOnLoad(InputManager.Instance.gameObject);
        DontDestroyOnLoad(AudioManager.Instance.gameObject);
        DontDestroyOnLoad(UIManager.Instance.gameObject);

        Application.targetFrameRate = 60;

        Init();
    }

    private void Init()
    {
        UIManager.PreloadUI<PlayUI>();
        UIManager.SetInteractable(false);
        UIManager.Instance.AsyncShow<LoadUI>(UILayer.Overlay, (ui) =>
        {
            ui.Fill = 0f;
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);
            sequence.Append(DOTweenUtils.DORunFloat((fill) => ui.Fill = fill, 0f, 1f, 1f).SetEase(Ease.InOutSine));
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() => new ClassicController());
            sequence.Append(ui.canvasGroup.DOFade(0f, 1f));
            sequence.AppendCallback(OnFinishLoading);

            void OnFinishLoading()
            {
                UIManager.SetInteractable(true);
                UIManager.ReleaseUI(ui);
            }
        }, true);
    }

}
