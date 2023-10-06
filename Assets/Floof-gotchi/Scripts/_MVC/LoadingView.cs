using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Floof
{
    public class LoadingView : BaseView
    {
        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _fillImg;
        [SerializeField] private RawImage _bgImg;
        [SerializeField] private Texture[] _bgTextures;

        public float Fill
        {
            get => _fillImg.fillAmount;
            set => _fillImg.fillAmount = value;
        }

        public override void OnShow()
        {
            _bgImg.texture = _bgTextures.GetRandom();
            _animator.Play(Anim.FloofEat);
            RunLoadingText();
        }

        private const string LoadingText = "Loading";
        private StringBuilder _stringBuilder = new StringBuilder(LoadingText);
        private int _lastDotCount;

        public void RunLoadingText()
        {
            StartCoroutine(RunLoadingTextRoutine());

            IEnumerator RunLoadingTextRoutine()
            {
                _loadingText.text = LoadingText;
                while (true)
                {
                    yield return null;

                    var dotCount = Mathf.RoundToInt((Time.time % 0.5f) * 10);
                    dotCount = Mathf.Clamp(dotCount, 0, 3);

                    if (_lastDotCount == dotCount) { continue; }
                    _lastDotCount = dotCount;

                    _stringBuilder.Length = LoadingText.Length;
                    for (int i = 0; i < dotCount; i++)
                    {
                        _stringBuilder.Append(".");
                    }

                    _loadingText.text = _stringBuilder.ToString();
                }
            }
        }

        public void SetText(string text)
        {
            StopAllCoroutines();
            _loadingText.text = text;
        }
    }
}