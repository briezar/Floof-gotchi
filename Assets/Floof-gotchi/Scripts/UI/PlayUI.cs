using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUI : BaseUI
{
    public override void OnBack()
    {
        UIManager.HideUI(this);
    }

}
