using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUI : BaseUI
{
    public override void OnBack()
    {
        UIManager.HideUI(this);
    }
}
