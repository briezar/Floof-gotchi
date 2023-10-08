using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class PlayView : BaseView
    {
        [SerializeField] private FloofPresenter _floofPrefab;
        [SerializeField] private Scene[] _scenePrefabs;
        [SerializeField] private Transform _bottomBar;
        [SerializeField] private CameraFollow _camFollow;

        public Scene CurrentScene { get; private set; }
        private Dictionary<NeedsType, NeedsInfo> _needs;
        private FloofPresenter _floof;

        public override void OnInstantiate()
        {
            var allNeeds = _bottomBar.GetComponentsInChildren<NeedsInfo>(true);

            _needs = new();
            for (int i = 0; i < allNeeds.Length; i++)
            {
                _needs.Add((NeedsType)i, allNeeds[i]);
            }

            _floof = Instantiate(_floofPrefab);

            _camFollow.SetTarget(_floof.transform);
        }

        public NeedsInfo GetNeeds(NeedsType needsType)
        {
            if (!_needs.TryGetValue(needsType, out var needsInfo))
            {
                Debug.LogWarning($"Invalid needs: {needsType}");
            }
            return needsInfo;
        }

        public void GoToScene(GameSceneType gameScene)
        {
            var index = (int)gameScene;
            CurrentScene.DestroyGameObject();
            CurrentScene = Instantiate(_scenePrefabs[index], ViewManager.Instance.WorldCanvas);

            var bounds = CurrentScene.GetBounds();
            _camFollow.SetBounds(bounds.min, bounds.max);
        }

        public override void OnBack()
        {
            // UIManager.ShowAsyncPopup<ConfirmPopup>();
        }

    }
}