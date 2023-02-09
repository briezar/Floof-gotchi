using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : BaseUI
{
    public override void OnHide()
    {
        UIManager.ReleaseUI(this);
    }

    public virtual void Click_Confirm()
    {

    }

    public virtual void Click_Deny()
    {

    }
}
