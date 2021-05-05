using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupUtility : MonoBehaviour
{
    [Header("General")]
    [SerializeField, Range(0, 5)] private float fadeDuration = 1;
    [Space]
    [SerializeField] private UnityEvent OnShowStart = null;
    [SerializeField] private UnityEvent OnShowFinish = null;
    [SerializeField] private UnityEvent OnHideStart = null;
    [SerializeField] private UnityEvent OnHideFinish = null;
    [SerializeField] private CanvasGroup _canvasGroup;

    private bool _isShowing;

    private void Start()
    {
        _isShowing = _canvasGroup.alpha == 1 ? true : false;
    }

    public void Show(bool isShowing)
    {
        _isShowing = isShowing;

        if (isShowing)
        {
            OnShowStart.Invoke();

            _canvasGroup
                .DOFade(1, fadeDuration)
                .OnComplete(() => SetProperties(true));
        }
        else
        {
            OnHideStart.Invoke();

            _canvasGroup
                .DOFade(0, fadeDuration)
                .OnComplete(() => CallFinishEvent(false));

            SetProperties(false);
        }
    }
    
    public void Show(bool isShowing, float duration)
    {
        _isShowing = isShowing;

        if (isShowing)
        {
            OnShowStart.Invoke();

            _canvasGroup
                .DOFade(1, duration)
                .OnComplete(() => SetProperties(true));
        }
        else
        {
            OnHideStart.Invoke();

            _canvasGroup
                .DOFade(0, duration)
                .OnComplete(() => CallFinishEvent(false));

            SetProperties(false);
        }
    }

    public void ShowInstant(bool isShowing)
    {
        _isShowing = isShowing;

        if (_isShowing)
        {
            _canvasGroup.alpha = 1;
            CallFinishEvent(true);
        }
        else
        {
            _canvasGroup.alpha = 0;
        }

        SetProperties(isShowing);
    }

    public void Toggle()
    {
        _isShowing = !_isShowing;

        Show(_isShowing);
    }

    private void SetProperties(bool isShowing)
    {
        _canvasGroup.interactable = isShowing;
        _canvasGroup.blocksRaycasts = isShowing;

        if (isShowing)CallFinishEvent(true);
    }

    private void CallFinishEvent(bool isShowing)
    {
        if (isShowing)
        {
            OnShowFinish.Invoke();
        }
        else
        {
            OnHideFinish.Invoke();
        }
    }

}