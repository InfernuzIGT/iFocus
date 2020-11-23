using DG.Tweening;
using UnityEngine;

public class AnimationFade : Animation
{
    private CanvasGroup _canvas;

    public override void Start()
    {
        _canvas = GetComponent<CanvasGroup>();
        
        base.Start();
    }

    public override void Show()
    {
        base.Show();

        _canvas.DOFade(1, _duration);
    }

    public override void Hide()
    {
        base.Hide();

        _canvas.DOFade(0, _duration);
    }

    public override void SetInteraction(bool interactuable)
    {
        _canvas.interactable = interactuable;
    }

}