using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RectTransform;

namespace Floof
{
    public class CameraFollow : MonoBehaviour
    {
        [field: SerializeField] public bool FollowHorizontal { get; private set; }
        [field: SerializeField] public bool FollowVertical { get; private set; }

        private Transform _target;

        private Vector3[] _boundCorners;

        private Vector3 _minBound => _boundCorners[0];
        private Vector3 _maxBound => _boundCorners[2];
        private Vector3 _minPoint => _cam.ViewportToWorldPoint(Vector3.zero);
        private Vector3 _maxPoint => _cam.ViewportToWorldPoint(Vector3.one);

        private Camera _cam => ViewManager.Instance.UICamera;

        public void SetTarget(Transform target)
        {
            _target = target;
            enabled = true;
        }

        public void SetBounds(RectTransform bound)
        {
            _boundCorners = bound.GetWorldCorners();
        }

        public void SetBounds(Vector3 minPos, Vector3 maxPos)
        {
            _boundCorners = new Vector3[4];
            _boundCorners[0] = minPos;
            _boundCorners[2] = maxPos;
        }

        private void Update()
        {
            if (_target == null)
            {
                enabled = false;
                return;
            }
            FollowTarget();
        }

        private void FollowTarget()
        {
            if (_boundCorners == null)
            {
                _cam.transform.position = _target.position;
                return;
            }

            var pos = _cam.transform.position;
            if (CanMoveHorizontal()) { pos.x = _target.position.x; }
            if (CanMoveVertical()) { pos.y = _target.position.y; }

            _cam.transform.position = pos;
        }

        private bool CanMoveHorizontal()
        {
            return FollowHorizontal && IsWithinBounds(Axis.Horizontal) || TargetPassedMidpoint(Axis.Horizontal);
        }

        private bool CanMoveVertical()
        {
            return FollowVertical && IsWithinBounds(Axis.Vertical) || TargetPassedMidpoint(Axis.Vertical);
        }

        private bool TargetPassedMidpoint(Axis axis)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    return Mathf.Abs(_target.position.x) < Mathf.Abs(_cam.transform.position.x);
                case Axis.Vertical:
                    return Mathf.Abs(_target.position.y) < Mathf.Abs(_cam.transform.position.y);
            }
            return false;
        }

        private bool IsWithinBounds(Axis axis)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    return _minPoint.x > _minBound.x && _maxPoint.x < _maxBound.x;
                case Axis.Vertical:
                    return _minPoint.y > _minBound.y && _maxPoint.y < _maxBound.y;
            }
            return false;
        }

    }
}