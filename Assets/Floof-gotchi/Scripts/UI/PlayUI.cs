using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicGame
{
    public enum Needs { Wellness, Hunger, Hygiene, Sleep }

    public class PlayUI : CanvasCameraUI
    {
        [SerializeField] private Transform _bottomBar;

        public Dictionary<Needs, NeedsIcon> NeedsIcons { get; private set; } = new();

        public override void OnInit()
        {
            var allNeeds = _bottomBar.GetComponentsInChildren<NeedsIcon>();
            for (int i = 0; i < allNeeds.Length; i++)
            {
                NeedsIcons.Add(Enum.Parse<Needs>(allNeeds[i].name), allNeeds[i]);
            }
        }

        public override void OnBack()
        {
            UIManager.ShowAsyncPopup<ConfirmPopup>();
        }

    }
}