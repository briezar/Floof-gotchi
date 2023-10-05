using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class ConfirmPopup : BaseView
    {
        public override void OnBack()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}