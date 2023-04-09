using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPopup : Popup
{
    public override void OnBack()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public override void Click_Confirm()
    {
        OnBack();
    }

    public override void Click_Deny()
    {
        OnHide();
    }

}
