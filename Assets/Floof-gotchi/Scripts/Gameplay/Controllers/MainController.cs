using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ClassicGame
{
    public class MainController
    {
        private PlayUI _playUI;
        private SceneUI _sceneUI;
        private FloofController _floofController;
        private SceneDataCenter _sceneDataCenter;

        public MainController()
        {
            _playUI = UIManager.ShowUI<PlayUI>();
            _sceneUI = UIManager.ShowWorldUI<SceneUI>();

            GameManager.Instance.StartCoroutine(InitRoutine());

            IEnumerator InitRoutine()
            {
                yield return AssetManager.LoadAssetByPath<TextAsset>("Data/SceneData.json", (asset) => _sceneDataCenter = JsonUtility.FromJson<SceneDataCenter>(asset.text));
                yield return AssetManager.InstantiateAsync<FloofView>("Prefabs/FloofView.prefab", _sceneUI.MoveSpace, (floofView) =>
                {
                    _sceneUI.SetupCameraFollow(floofView.transform);
                    _sceneUI.SetupSceneData(_sceneDataCenter);
                    _sceneUI.OnChangeBG = () =>
                    {
                        floofView.StopMoving();
                        floofView.Setup(_sceneUI.MoveSpace);
                        floofView.StartWandering(2f);
                    };
                    _sceneUI.ChangeBG(GameScene.LivingRoom);
                    _floofController = new FloofController(floofView);
                });
            }
        }
    }
}