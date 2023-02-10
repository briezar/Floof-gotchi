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
        DontDestroyOnLoad(AudioManager.Instance.gameObject);
        DontDestroyOnLoad(UIManager.Instance.gameObject);

        Application.targetFrameRate = 60;

        Init();
    }

    private void Init()
    {
        StartCoroutine(InitRoutine());
        IEnumerator InitRoutine()
        {
            UIManager.SetInteractable(false);

            yield return UIManager.Instance.PreloadUIsRoutine((percent) => Debug.Log(percent));
            var loadUI = UIManager.ShowUI<LoadUI>(UILayer.Overlay);

            loadUI.Fill = 0f;
            var sequence = DOTween.Sequence()
            .Append(loadUI.canvasGroup.DOFade(1f, 1f).ChangeStartValue(0f))
            .Append(DOTweenUtils.DORunFloat((fill) => loadUI.Fill = fill, 0f, 1f, 1f).SetEase(Ease.InOutSine))
            .AppendInterval(0.5f)
            .AppendCallback(() => new ClassicController())
            .Append(loadUI.canvasGroup.DOFade(0f, 1f))
            .AppendCallback(OnFinishLoading);

            void OnFinishLoading()
            {
                UIManager.SetInteractable(true);
                UIManager.ReleaseUI(loadUI);
            }
        }
    }

}
