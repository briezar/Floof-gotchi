using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Floof
{
    public class FloofPresenter : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speed;

        private Vector3[] _moveSpaceCorners;

        private Coroutine _wanderRoutine;
        private Tween _moveTween;

        public void SetMoveSpace(RectTransform moveSpace)
        {
            transform.position = moveSpace.position;
            _moveSpaceCorners = moveSpace.GetWorldCorners();
        }

        public void StartWandering(float delay = 0)
        {
            StopMoving();
            _wanderRoutine = StartCoroutine(MoveRoutine());
            IEnumerator MoveRoutine()
            {
                yield return new WaitForSeconds(delay);
                while (true)
                {
                    yield return Move(GetRandomPos());
                    yield return new WaitForSeconds(Random.Range(1f, 4f));
                }
            }
        }

        public YieldInstruction Move(Vector3 destination)
        {
            _animator.Play(Anim.FloofWalk);
            bool isFacingRight = transform.position.x < destination.x;
            transform.localScale = new Vector3(isFacingRight ? 1 : -1, transform.localScale.y);

            var distance = Vector3.Distance(transform.position, destination);
            var moveTime = distance / _speed;

            _moveTween = transform.DOMove(destination, moveTime.ClampMax(3)).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _animator.Play(Anim.FloofIdle0);
            });
            return _moveTween.WaitForCompletion();
        }

        public void StopMoving()
        {
            if (_wanderRoutine != null) { StopCoroutine(_wanderRoutine); }
            _moveTween.Kill();
        }

        private Vector2 GetRandomPos()
        {
            var minPos = _moveSpaceCorners[0];
            var maxPos = _moveSpaceCorners[2];

            var x = Random.Range(minPos.x, maxPos.x);
            var y = Random.Range(minPos.y, maxPos.y);
            var pos = new Vector2(x, y);

            return pos;
        }

    }
}