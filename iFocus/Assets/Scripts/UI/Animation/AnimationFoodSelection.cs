using DG.Tweening;
using UnityEngine;

public class AnimationFoodSelection : Animation
{
    [Header("Food Selection")]
    [SerializeField] private RectTransform _background = null;
    [SerializeField] private CanvasGroup _container = null;
    [SerializeField] private CanvasGroup _fade = null;

    public override void Start()
    {
        base.Start();
    }

    public override void Show()
    {
        base.Show();

        _background.DOScale(Vector2.one, _duration)
            .SetEase(_ease);

        _container.DOFade(1, _duration);
        _fade.DOFade(1, _duration);
    }

    public override void Hide()
    {
        base.Hide();

        _background.DOScale(Vector2.zero, _duration)
            .SetDelay(_delay)
            .SetEase(_ease);

        _fade.DOFade(0, _duration);
        _container.DOFade(0, _duration);
    }

    public override void SetInteraction(bool interactuable)
    {
        _container.interactable = interactuable;
    }

}