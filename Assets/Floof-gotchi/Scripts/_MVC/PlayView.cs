using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class PlayView : BaseView
    {
        [field: SerializeField] public FloofEntity Floof { get; private set; }

        [SerializeField] private Transform _sceneHolder;
        [SerializeField] private Scene[] _scenePrefabs;
        [SerializeField] private Transform _bottomBar;

        public Scene CurrentScene { get; private set; }
        public Dictionary<Needs, NeedsInfo> Needs { get; private set; } = new();

        public override void OnInstantiate()
        {
            var allNeeds = _bottomBar.GetComponentsInChildren<NeedsInfo>(true);
            for (int i = 0; i < allNeeds.Length; i++)
            {
                Needs.Add((Needs)i, allNeeds[i]);
            }
        }

        public void GoToScene(GameScene gameScene)
        {
            var index = (int)gameScene;
            CurrentScene.DestroyGameObject();
            CurrentScene = Instantiate(_scenePrefabs[index], ViewManager.Instance.WorldCanvas);
            CurrentScene.Setup(Floof.transform);
        }

        public override void OnBack()
        {
            // UIManager.ShowAsyncPopup<ConfirmPopup>();
        }

    }
}