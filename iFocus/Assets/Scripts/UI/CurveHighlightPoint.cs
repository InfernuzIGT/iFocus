using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class CurveHighlightPoint : MonoBehaviour
{
    public int Index { get; set; }

    private Image image;

    public void SetPosition(Vector2 newPosition)
    {
        image.rectTransform.position = newPosition;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

}
