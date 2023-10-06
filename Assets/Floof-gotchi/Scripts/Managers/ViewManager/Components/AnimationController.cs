using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Floof.ViewManagerControllers
{
    [Serializable]
    public class AnimationController : CoroutineDelegate
    {
        [SerializeField] private Loading _loading;
        [SerializeField] private CanvasGroup _popupBgDim;
        [SerializeField] private SpriteRenderer _screenFaderSprite;
        [SerializeField] private CameraShake _cameraShake;

        public void ShakeCamera(float duration, float strength = 12, int vibrato = 15)
        {
            _cameraShake.ShakeCamera(duration, strength, vibrato);
        }

        public void FadeTransition(FadeSetting fadeSetting)
        {
            _coroutineRunner ??= ViewManager.Instance;

            var faderSprite = _screenFaderSprite;

            if (faderSprite.gameObject.activeInHierarchy)
            {
                Debug.LogWarning("Currently playing fade anim, cannot start!");
                return;
            }

            faderSprite.gameObject.SetActive(true);

            StartCoroutine(FadeRoutine());
            IEnumerator FadeRoutine()
            {
                ViewManager.SetInteractable(false);
                faderSprite.SetAlpha(0);
                yield return faderSprite.DOFade(1, fadeSetting.FadeInDuration).WaitForCompletion();
                fadeSetting.OnFadeInComplete?.Invoke();

                yield return YieldCollection.WaitForSeconds(fadeSetting.WaitAfterFadeIn);
                if (fadeSetting.FadeOutCondition != null)
                {
                    while (!fadeSetting.FadeOutCondition()) { yield return null; }
                }
                fadeSetting.OnFadeOutStart?.Invoke();
                yield return faderSprite.DOFade(0, fadeSetting.FadeOutDuration).WaitForCompletion();
                faderSprite.gameObject.SetActive(false);

                fadeSetting.OnFinish?.Invoke();
                ViewManager.SetInteractable(true);
            }
        }

        public void ShowPopupBackgroundDim(float fadeInDuration = 0)
        {
            _popupBgDim.DOKill();
            _popupBgDim.gameObject.SetActive(true);
            _popupBgDim.DOFade(1, fadeInDuration);
        }
        public void HidePopupBackgroundDim(float fadeOutDuration = 0)
        {
            _popupBgDim.DOKill();
            _popupBgDim.DOFade(0, fadeOutDuration).OnComplete(() => _popupBgDim.gameObject.SetActive(false));
        }

        public void EnableLoading(bool enable, string text = "")
        {
            _loading.gameObject.SetActive(enable);
            _loading.Text.text = text;
        }
    }
}
