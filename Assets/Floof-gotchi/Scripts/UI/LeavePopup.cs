using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavePopup : Popup
{
    public override void OnBack()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

}
