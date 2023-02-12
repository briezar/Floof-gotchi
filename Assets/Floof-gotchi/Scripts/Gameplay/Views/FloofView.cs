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
        StartMovingRandomly();
    }

    public void StartMovingRandomly()
    {
        StartCoroutine(MoveRoutine());
        IEnumerator MoveRoutine()
        {
            while (true)
            {
                _animator.Play(Anim.FloofWalk);
                var destination = GetRandomLocalPos();
                bool isFacingRight = transform.localPosition.x < destination.x;
                transform.localScale = new Vector3(isFacingRight ? 1 : -1, transform.localScale.y);
                yield return transform.DOLocalMove(destination, 100f).SetSpeedBased().WaitForCompletion();
                _animator.Play(Anim.FloofIdle);
                yield return new WaitForSeconds(Random.Range(1f, 4f));
            }
        }
    }

    public void StopMoving()
    {
        transform.DOKill();
        _animator.Play(Anim.FloofIdle);
    }

    private Vector2 GetRandomLocalPos()
    {
        return Utils.GetRandomPoint(_min, _max);
    }

}
