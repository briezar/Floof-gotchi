using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public enum ShowPopupBehaviour
    {
        DoNothing,
        HideLowerPopup,
    }


    public class FadeSetting
    {
        public float FadeInDuration;
        public float FadeOutDuration;
        public float WaitAfterFadeIn;
        public Action OnFadeInComplete;
        public Action OnFadeOutStart;
        public Action OnFinish;
        public Func<bool> FadeOutCondition;

        public static FadeSetting Default => new FadeSetting(ViewManager.DefaultTransitionDuration, ViewManager.DefaultTransitionDuration);
        public static FadeSetting FadeOut(float fadeOutDuration = ViewManager.DefaultTransitionDuration)
        {
            var fadeSetting = new FadeSetting(0, fadeOutDuration);
            return fadeSetting;
        }
        public static FadeSetting FadeIn(float fadeInDuration = ViewManager.DefaultTransitionDuration, float waitAfterFadeIn = 0)
        {
            var fadeSetting = new FadeSetting(fadeInDuration, 0, waitAfterFadeIn);
            return fadeSetting;
        }

        public FadeSetting(float fadeInDuration, float fadeOutDuration, float waitAfterFadeIn = 0)
        {
            FadeInDuration = fadeInDuration;
            FadeOutDuration = fadeOutDuration;
            WaitAfterFadeIn = waitAfterFadeIn;
        }
    }
}