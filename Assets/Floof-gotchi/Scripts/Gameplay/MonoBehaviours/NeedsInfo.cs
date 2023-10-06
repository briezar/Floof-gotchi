using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedsInfo : MonoBehaviour
{
    [SerializeField] private Image _foregroundImg;

    public float Fill
    {
        get => _foregroundImg.fillAmount;
        set => _foregroundImg.fillAmount = value;
    }

}
