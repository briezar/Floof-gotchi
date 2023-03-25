using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUI : CanvasCameraUI
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _fillImg;
    [SerializeField] private RawImage _bgImg;
    [SerializeField] private Texture[] _bgTextures;

    public float Fill
    {
        get => _fillImg.fillAmount;
        set => _fillImg.fillAmount = value;
    }

    private void OnEnable()
    {
        _bgImg.texture = _bgTextures.GetRandom();
        _animator.Play(Anim.FloofEat);
    }
}
