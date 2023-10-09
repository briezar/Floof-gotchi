using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class PlayView : BaseView
    {
        [SerializeField] private Transform _bottomBar;

        private Dictionary<NeedsType, NeedsInfo> _needs;

        public override void OnInstantiate()
        {
            var allNeeds = _bottomBar.GetComponentsInChildren<NeedsInfo>(true);

            _needs = new();
            for (int i = 0; i < allNeeds.Length; i++)
            {
                _needs.Add((NeedsType)i, allNeeds[i]);
            }
        }

        public NeedsInfo GetNeeds(NeedsType needsType)
        {
            if (!_needs.TryGetValue(needsType, out var needsInfo))
            {
                Debug.LogWarning($"Invalid needs: {needsType}");
            }
            return needsInfo;
        }


        public override void OnBack()
        {
            // UIManager.ShowAsyncPopup<ConfirmPopup>();
        }

    }
}