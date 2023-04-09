using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SceneUI : WorldCameraUI
{
    [field: SerializeField] public RectTransform MoveSpace { get; private set; }
    [SerializeField] private Image _bgImage;
    [SerializeField] private AssetReferenceSprite[] _bgSpriteRefs;

    private CameraFollow _camFollow;

    private Dictionary<GameScene, SceneData> _sceneDatas;


    public Action OnChangeBG;

    private void Awake()
    {
        var length = _bgSpriteRefs.Length;

        for (int i = 0; i < _bgSpriteRefs.Length; i++)
        {
            _bgSpriteRefs[i].LoadAssetAsync<Sprite>();
        }
    }

    private void OnDestroy()
    {
        foreach (var assetRef in _bgSpriteRefs)
        {
            assetRef.ReleaseAsset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeBG(GameScene.LivingRoom);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeBG(GameScene.BathRoom);
        }
    }

    public void SetupSceneData(SceneDataCenter sceneDataCenter)
    {
        _sceneDatas = new();
        var length = sceneDataCenter.SceneDatas.Length;
        for (int i = 0; i < length; i++)
        {
            var sceneData = sceneDataCenter.SceneDatas[i];

            _sceneDatas.Add(sceneData.Scene, sceneData);
        }
        _bgImage.preserveAspect = false;
    }

    public void ChangeBG(GameScene scene)
    {
        StartCoroutine(ChangeBGRoutine());

        IEnumerator ChangeBGRoutine()
        {
            foreach (var assetRef in _bgSpriteRefs)
            {
                if (!assetRef.IsDone)
                {
                    yield return null;
                }
            }
            _bgImage.sprite = (Sprite)_bgSpriteRefs[(int)scene].Asset;
            MakeImageEnvelopeParent();
            _camFollow.SetBounds(_bgImage.rectTransform);

            var moveSpace = _sceneDatas[scene].MoveSpace;
            MoveSpace.localPosition = new Vector3(moveSpace.x, moveSpace.y);
            MoveSpace.sizeDelta = new Vector2(moveSpace.width, moveSpace.height);
            OnChangeBG?.Invoke();
        }
    }

    public void SetupCameraFollow(Transform target)
    {
        _camFollow ??= this.ForceGetComponent<CameraFollow>();
        _camFollow.FollowHorizontal = true;
        MakeImageEnvelopeParent();
        _camFollow.SetBounds(_bgImage.rectTransform);
        _camFollow.SetTarget(target);
    }

    private void MakeImageEnvelopeParent()
    {
        _bgImage.rectTransform.FitInParent(_bgImage.sprite.GetAspectRatio(), true);
    }

}
