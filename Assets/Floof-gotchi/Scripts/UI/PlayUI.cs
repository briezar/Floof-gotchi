using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Needs { Wellness, Hungry, Hygiene, Sleep }

public class PlayUI : BaseUI
{
    [SerializeField] Transform _bottomBar;

    public Dictionary<Needs, NeedsIcon> NeedsIcons { get; private set; } = new();

    private void Awake()
    {
        var allNeeds = _bottomBar.GetComponentsInChildren<NeedsIcon>();
        for (int i = 0; i < allNeeds.Length; i++)
        {
            NeedsIcons.Add(Enum.Parse<Needs>(allNeeds[i].name), allNeeds[i]);
        }
    }

    public override void OnBack()
    {
        UIManager.ShowPopup<LeavePopup>();
    }

}
