using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public abstract class PopupView : BaseView
    {
        [field: SerializeField] public ShowPopupBehaviour ShowPopupBehaviour { get; private set; } = ShowPopupBehaviour.HideLowerPopup;
    }
}