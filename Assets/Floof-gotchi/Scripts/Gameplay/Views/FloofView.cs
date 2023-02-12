using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloofView : MonoBehaviour
{
    [SerializeField] private RectTransform _moveSpace;
    [SerializeField] private Animator _animator;

    private Vector2 _min, _max;

    private void Awake()
    {
        _min = _moveSpace.rect.min;
        _max = _moveSpace.rect.max;

        InvokeRepeating(nameof(MoveToRandomPoint), 1f, 5f);
    }

    public void MoveToRandomPoint()
    {
        _animator.Play(Anim.FloofWalk);
        transform.DOLocalMove(GetRandomLocalPos(), 100f).SetSpeedBased().OnComplete(() => _animator.Play(Anim.FloofIdle));
    }

    private Vector2 GetRandomLocalPos()
    {
        return Utils.GetRandomPoint(_min, _max);
    }

}
