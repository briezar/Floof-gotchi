using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoroutineUtils
{
    /// <summary> 1d 23h or 23h 45m or 3m 4s </summary>
    public static IEnumerator DH_HM_MS_TimerRoutine(TMP_Text timerText, DateTime timeEnd, Action onTimerEnd = null, string extraFormat = null)
    {
        TimeSpan timeRemain = (timeEnd - DateTime.Now);

        var time = string.Empty;
        while (timeRemain.TotalSeconds > 0)
        {
            var d = (int)timeRemain.TotalDays;
            var h = timeRemain.Hours;
            var m = timeRemain.Minutes;
            var s = timeRemain.Seconds;

            if (d > 0)
            {
                time = $"{d}d {h}h";
            }
            else if (h > 0)
            {
                time = $"{h}h {m}m";
            }
            else
            {
                time = $"{m}m {s}s";
            }

            timerText.text = (extraFormat == null ? time : extraFormat.Format(time));

            yield return YieldCollection.WaitForSeconds(1);
            timeRemain = timeEnd - DateTime.Now;
        }

        onTimerEnd?.Invoke();
    }

    /// <summary> 23:34:45 </summary>
    public static IEnumerator HMS_TimerRoutine(TMP_Text timerText, DateTime timeEnd, Action onTimerEnd = null, string extraFormat = null)
    {
        TimeSpan timeRemain = (timeEnd - DateTime.Now);

        var time = string.Empty;
        while (timeRemain.TotalSeconds > 0)
        {
            var h = (int)timeRemain.TotalHours;
            var m = timeRemain.Minutes;
            var s = timeRemain.Seconds;

            time = $"{h:00}:{m:00}:{s:00}";

            timerText.text = (extraFormat == null ? time : extraFormat.Format(time));

            yield return YieldCollection.WaitForSeconds(1);
            timeRemain = timeEnd - DateTime.Now;
        }

        onTimerEnd?.Invoke();
    }

    public static IEnumerator WaitUntil(Func<bool> condition, Action callback)
    {
        while (true)
        {
            if (condition == null ? true : condition())
            {
                callback?.Invoke();
                yield break;
            }
            yield return null;
        }
    }
    public static IEnumerator WaitClick(Action onClick, bool checkCanClick = true, Func<bool> condition = null)
    {
        yield return null;
        while (true)
        {
            if ((condition == null ? true : condition()) && Input.GetMouseButtonDown(0))
            {
                onClick?.Invoke();
                yield break;
            }
            yield return null;
        }
    }

    public static IEnumerator RepeatInterval(float interval, Action action)
    {
        while (true)
        {
            yield return YieldCollection.WaitForSeconds(interval);
            action?.Invoke();
        }
    }
}
