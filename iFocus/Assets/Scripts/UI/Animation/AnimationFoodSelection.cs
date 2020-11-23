using DG.Tweening;
using UnityEngine;

public class AnimationFoodSelection : Animation
{
    [Header("Food Selection")]
    [SerializeField] private RectTransform _background = null;
    [SerializeField] private CanvasGroup _container = null;

    public override void Start()
    {
        base.Start();
    }

    public override void Show()
    {
        SetInteraction(true);

        _background.DOScale(Vector2.one, _duration)
            .SetEase(_ease);

        _container.DOFade(1, _duration);
    }

    public override void Hide()
    {
        SetInteraction(false);

        _background.DOScale(Vector2.zero, _duration)
            .SetDelay(_delay)
            .SetEase(_ease);

        _container.DOFade(0, _duration);
    }

    public override void SetInteraction(bool interactuable)
    {
        _container.interactable = interactuable;
    }

}