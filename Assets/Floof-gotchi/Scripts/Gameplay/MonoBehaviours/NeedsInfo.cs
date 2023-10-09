using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Floof.Constants;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Floof
{
    public class NeedsInfo : MonoBehaviour
    {
        [SerializeField] private Image _foregroundImg;

        private Button _button;

        private IEnumerator Start()
        {
            Fill = 0;
            while (true)
            {
                SmoothFill(Random.value);
                yield return new WaitForSeconds(3f);
            }
        }

        public float Fill
        {
            get => _foregroundImg.fillAmount;
            set
            {
                _foregroundImg.fillAmount = value;

                Color color;
                if (value > GameConfig.Needs.NormalThreshold)
                {
                    color = GameConfig.Needs.HighColor;
                }
                else if (value > GameConfig.Needs.LowThreshold)
                {
                    color = GameConfig.Needs.NormalColor;
                }
                else
                {
                    color = GameConfig.Needs.LowColor;
                }

                _foregroundImg.color = color;
            }
        }

        public Tween SmoothFill(float amountNormalized, float speed = 0.33f)
        {
            DOTween.Kill(GetInstanceID());

            var current = Fill;

            var tween = DOVirtual.Float(current, amountNormalized, speed, (value) =>
            {
                Fill = value;
            })
            .SetSpeedBased()
            .SetId(GetInstanceID());

            return tween;
        }

        public void SetOnClick(Action onClick)
        {
            _button ??= GetComponent<Button>();
            _button.SetOnClick(onClick);
        }

    }
}
