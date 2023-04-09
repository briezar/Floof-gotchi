using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.IO;

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
        UIManager.SetInteractable(false);

        var loadUI = UIManager.ShowUI<LoadUI>(UILayer.Overlay);
        StartCoroutine(InitRoutine(loadUI));

        IEnumerator InitRoutine(LoadUI loadUI)
        {
            loadUI.Fill = 0f;
            float maxFill = 0f;
            UIManager.Instance.PreloadUIsRoutine((percent) =>
            {
                maxFill = percent;
            });

            yield return loadUI.canvasGroup.DOFade(1f, 0.75f).ChangeStartValue(0f).SetDelay(0.25f).WaitForCompletion();

            while (loadUI.Fill < 0.99f)
            {
                if (loadUI.Fill < maxFill)
                {
                    loadUI.Fill += Time.deltaTime;
                }
                yield return null;
            }
            loadUI.Fill = 1f;

            var sequence = DOTween.Sequence()
            .AppendCallback(() => new ClassicController())
            .AppendInterval(0.25f)
            .Append(loadUI.canvasGroup.DOFade(0f, 0.75f));

            yield return sequence.WaitForCompletion();

            UIManager.SetInteractable(true);
            UIManager.ReleaseUI(loadUI);
        }
    }

}
