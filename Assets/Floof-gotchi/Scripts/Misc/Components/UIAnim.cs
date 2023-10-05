using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Floof
{
    public class UIAnim : MonoBehaviour
    {
        private CanvasGroup _cacheCanvasGroup;

        public RectTransform rectTransform => transform as RectTransform;
        public CanvasGroup canvasGroup => _cacheCanvasGroup ??= this.ForceGetComponent<CanvasGroup>();

        private Vector3 _originalScale;

        private HashSet<string> _tweenIds = new();

        /// <summary> (Fade in) + (EaseOutBack scale)  </summary>
        public Tween PlayAppear(float duration = ViewManager.DefaultTransitionDuration, float scaleUpFactor = 1.5f)
        {
            var tweenId = GetTweenId(nameof(PlayAppear));
            _tweenIds.Add(tweenId);

            if (_originalScale == Vector3.zero) { _originalScale = transform.localScale; }

            canvasGroup.alpha = 0;
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => { KillOtherTweensAndActivate(tweenId); })
            .Append(canvasGroup.DOFade(1, duration * 0.5f))
            .Join(transform.DOScale(_originalScale, duration).ChangeStartValue(_originalScale * scaleUpFactor).SetEase(Ease.OutBack))
            .SetId(tweenId);

            return sequence;
        }

        /// <summary> (Fade out) + (Scale)  </summary>
        public Tween PlayDisappear(float duration = ViewManager.DefaultTransitionDuration, float scaleUpFactor = 1.25f)
        {
            var tweenId = GetTweenId(nameof(PlayDisappear));
            _tweenIds.Add(tweenId);

            if (_originalScale == Vector3.zero) { _originalScale = transform.localScale; }

            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => { KillOtherTweensAndActivate(tweenId); })
            .Append(canvasGroup.DOFade(0, duration * 0.95f))
            .Join(transform.DOScale(_originalScale * scaleUpFactor, duration))
            .AppendCallback(() => gameObject.SetActive(false))
            .SetId(tweenId);

            return sequence;
        }

        /// <summary> Fade out </summary>
        public Tween FadeOut(float duration = ViewManager.DefaultTransitionDuration)
        {
            var tweenId = GetTweenId(nameof(FadeOut));
            _tweenIds.Add(tweenId);

            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => KillOtherTweensAndActivate(tweenId))
            .Append(canvasGroup.DOFade(0, duration))
            .AppendCallback(() => gameObject.SetActive(false))
            .SetId(tweenId);

            return sequence;
        }

        private string GetTweenId(string animName)
        {
            var id = gameObject.GetInstanceID() + animName;
            return id;
        }

        private void KillOtherTweensAndActivate(string currentAnim)
        {
            foreach (var id in _tweenIds)
            {
                if (id == currentAnim) { continue; }
                DOTween.Kill(GetTweenId(id));
            }
            gameObject.SetActive(true);
        }

    }
}