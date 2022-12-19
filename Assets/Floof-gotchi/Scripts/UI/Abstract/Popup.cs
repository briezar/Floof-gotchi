using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : BaseUI
{
    public override void OnHide()
    {
        Debug.Log("on hide");
        UIManager.ReleaseUI(this);
    }
}
