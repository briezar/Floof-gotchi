using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum GameScene
{
    LivingRoom,
    BathRoom
}
public class ClassicController
{
    private PlayUI _playUI;
    private SceneUI _sceneUI;
    private FloofController _floofController;
    private SceneDataCenter _sceneDataCenter;

    public ClassicController()
    {
        _playUI = UIManager.ShowUI<PlayUI>();
        _sceneUI = UIManager.ShowWorldUI<SceneUI>();

        GameManager.Instance.StartCoroutine(InitRoutine());

        IEnumerator InitRoutine()
        {
            yield return AssetManager.LoadTextAsync("Data/SceneData.json", (text) => _sceneDataCenter = JsonUtility.FromJson<SceneDataCenter>(text));
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
