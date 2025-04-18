using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private RectTransform rectTrans;
    public Vector2Int CurrentCell { get; set; }
    public bool IsMoving { get; private set; }

    public IEnumerator IEMoveAlongPath(IReadOnlyList<Vector2> pathPoints, Action onComplete)
    {
        for (var i = 0; i < pathPoints.Count - 1; i++)
        {
            var currentPoint = pathPoints[i];
            rectTrans.anchoredPosition = currentPoint;
            var nextPoint = pathPoints[i + 1];

            var dir = (nextPoint - currentPoint).normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Rotate(angle - 90);

            yield return IEMoveToDestination(nextPoint);
        }

        onComplete.Invoke();
    }

    public IEnumerator IEMoveToDestination(Vector2 destination, Action onComplete = null)
    {
        IsMoving = true;
        yield return rectTrans.DOAnchorPos(destination,
            (destination - rectTrans.anchoredPosition).magnitude / moveSpeed).SetEase(Ease.Linear).WaitForCompletion();
        onComplete?.Invoke();
        IsMoving = false;
    }

    public void Rotate(float angle)
    {
        rectTrans.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetPosition(Vector2 position)
    {
        rectTrans.anchoredPosition = position;
    }

    private void OnDisable()
    {
        IsMoving = false;
    }
}