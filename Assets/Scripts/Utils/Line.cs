using System;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private RectTransform lineRect;

    public void InitLine(Vector2 from, Vector2 to)
    {
        var sizeDelta = lineRect.sizeDelta;

        //scaleY
        if (Math.Abs(from.x - to.x) < 0.01)
        {
            lineRect.anchoredPosition = new Vector2(from.x - sizeDelta.x / 2, from.y);
            var dif = to.y - from.y;
            lineRect.localScale = new Vector3(1, dif / sizeDelta.y + (dif > 0 ? 0.5f : -0.5f), 1);
        }
        //scaleX
        else if (Math.Abs(from.y - to.y) < 0.01)
        {
            lineRect.anchoredPosition = new Vector2(from.x, from.y - sizeDelta.y / 2);
            var dif = to.x - from.x;
            lineRect.localScale = new Vector3(dif / sizeDelta.x + (dif > 0 ? 0.5f : -0.5f), 1, 1);
        }
    }
}