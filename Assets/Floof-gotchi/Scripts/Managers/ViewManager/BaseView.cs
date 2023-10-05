using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace Floof
{
    public abstract class BaseView : MonoBehaviour
    {
        [field: SerializeField] public bool IsPopup { get; private set; }
        [field: SerializeField] public bool DestroyOnHide { get; private set; }

        [ShowIf(nameof(IsPopup))]
        [SerializeField] private ShowPopupBehaviour _showPopupBehaviour = ShowPopupBehaviour.HideLowerPopup;

        public ShowPopupBehaviour ShowPopupBehaviour => _showPopupBehaviour;

        private CanvasGroup _cacheCanvasGroup;
        public CanvasGroup canvasGroup => _cacheCanvasGroup ??= this.ForceGetComponent<CanvasGroup>();
        private UIAnim _cacheUIanim;
        public UIAnim UIAnim => _cacheUIanim ??= this.ForceGetComponent<UIAnim>();

        /// <summary> Used to block user interaction when transitioning </summary>
        public virtual float TransitionInDuration => ViewManager.DefaultTransitionDuration;

        /// <summary> Used to block user interaction when transitioning </summary>
        public virtual float TransitionOutDuration => 0;

        /// <summary> Called once only, before OnShow </summary>
        public virtual void OnInstantiate() { }

        /// <summary> Called every ViewManager.Show() </summary>
        public virtual void OnShow() { }
        public virtual void OnHide() { }
        public virtual void OnBack() { }

        /// <summary> Heavy operation, should not abuse </summary>
        protected Tween FadeInAllSpriteRenderers(float duration = ViewManager.DefaultTransitionDuration, bool includeInactive = false)
        {
            var sequence = DOTween.Sequence();
            foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>(includeInactive))
            {
                if (!spriteRenderer.material.HasProperty("_Color")) { continue; }
                sequence.Join(spriteRenderer.material.DOFade(0, duration).From());
            }
            return sequence;
        }
        /// <summary> Heavy operation, should not abuse </summary>
        protected Tween FadeOutAllSpriteRenderers(float duration = ViewManager.DefaultTransitionDuration, bool includeInactive = false)
        {
            var sequence = DOTween.Sequence();
            foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>(includeInactive))
            {
                if (!spriteRenderer.material.HasProperty("_Color")) { continue; }
                sequence.Join(spriteRenderer.material.DOFade(0, duration));
            }
            return sequence;
        }

        protected Tween FadeIn(float duration = ViewManager.DefaultTransitionDuration)
        {
            canvasGroup.DOKill();
            return canvasGroup.DOFade(1, duration).ChangeStartValue(0);
        }

        protected Tween FadeOut(float duration = ViewManager.DefaultTransitionDuration)
        {
            canvasGroup.DOKill();
            return canvasGroup.DOFade(0, duration).ChangeStartValue(1);
        }
    }
}