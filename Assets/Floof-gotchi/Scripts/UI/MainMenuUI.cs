using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : BaseUI
{
    public override void OnBack()
    {
        UIManager.ShowPopup<LeavePopup>();
    }

}
