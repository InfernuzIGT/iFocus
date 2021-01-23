using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPointUISet : MonoBehaviour
{
    public ButtonTimeline[] _highlightPoints;

    public void ToggleAll(bool visible)
    {
        for (int i = 0; i < _highlightPoints.Length; i++)
        {
            _highlightPoints[i].IsSelected = visible;
        }
    }
}