using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI : WorldCameraUI
{
    [field: SerializeField] public RectTransform MoveSpace { get; private set; }
    [SerializeField] private Image _bgImage;

    private CameraFollow _camFollow;

    public void ChangeBG(Sprite sprite)
    {
        _bgImage.sprite = sprite;
        MakeImageEnvelopeParent();
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
