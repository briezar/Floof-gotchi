using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClassicController
{
    private PlayUI _playUI;
    private SceneUI _sceneUI;
    private FloofController _floofController;

    public ClassicController()
    {
        _playUI = UIManager.ShowUI<PlayUI>();
        _sceneUI = UIManager.ShowWorldUI<SceneUI>();
        AssetManager.InstantiateAsync<FloofView>("Prefabs/FloofView.prefab", _sceneUI.MoveSpace, (floofView) =>
        {
            _sceneUI.SetupCameraFollow(floofView.transform);
            floofView.Setup(_sceneUI.MoveSpace);
            _floofController = new FloofController(floofView);
        });
    }

}
