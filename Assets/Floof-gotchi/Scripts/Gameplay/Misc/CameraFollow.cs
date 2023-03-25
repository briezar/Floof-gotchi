using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private RectTransform _bound;
    [SerializeField] public bool FollowHorizontal;
    [SerializeField] public bool FollowVertical;

    private Vector3[] _boundCorners;
    private Vector3 _minBound, _maxBound;
    private Vector3 _minPoint => _cam.ViewportToWorldPoint(Vector3.zero);
    private Vector3 _maxPoint => _cam.ViewportToWorldPoint(Vector3.one);

    private Camera _cam => UIManager.Instance.UICamera;
    private enum Axis
    {
        Horizontal,
        Vertical
    }

    private void Awake()
    {
        UpdateBounds();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        enabled = true;
    }

    public void SetBounds(RectTransform bound)
    {
        _bound = bound;
        UpdateBounds();
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

    private void UpdateBounds()
    {
        if (_bound == null) { return; }
        _minBound = _bound.TransformPoint(_bound.rect.min);
        _maxBound = _bound.TransformPoint(_bound.rect.max);
    }

    private void FollowTarget()
    {
        if (_bound == null)
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
        return FollowHorizontal && IsWithinBounds(Axis.Horizontal) || (!IsWithinBounds(Axis.Horizontal) && TargetPassedMidpoint(Axis.Horizontal));
    }

    private bool CanMoveVertical()
    {
        return FollowVertical && IsWithinBounds(Axis.Vertical) || (!IsWithinBounds(Axis.Vertical) && TargetPassedMidpoint(Axis.Vertical));
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
