using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationButtonMaster : Animation
{
    [Header("Button Master")]
    [SerializeField] private VerticalLayoutGroup _verticalLayout = null;
    [SerializeField] private CanvasGroup _buttonReset = null;
    [SerializeField] private CanvasGroup _buttonSkip = null;

    public override void Start()
    {
        base.Start();
        
        // Fix Sorting Order
        _buttonReset.GetComponent<Canvas>().sortingOrder = 18;
        _buttonSkip.GetComponent<Canvas>().sortingOrder = 16;
    }

    public override void Show()
    {
        base.Show();

        DOTween.To(
            () => _verticalLayout.spacing,
            x => _verticalLayout.spacing = x,
            0,
            _duration);

        _buttonReset.DOFade(1, _duration);
        _buttonSkip.DOFade(1, _duration);
    }

    public override void Hide()
    {
        base.Hide();

        DOTween.To(
            () => _verticalLayout.spacing,
            x => _verticalLayout.spacing = x, -74,
            _duration);

        _buttonReset.DOFade(0, _duration);
        _buttonSkip.DOFade(0, _duration);
    }

    public override void SetInteraction(bool interactuable)
    {
        _buttonReset.interactable = interactuable;
        _buttonSkip.interactable = interactuable;
    }
}