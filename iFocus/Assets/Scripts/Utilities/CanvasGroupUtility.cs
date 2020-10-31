﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupUtility : MonoBehaviour
{
    [Header("General")]
    [SerializeField, Range(0,5)] private float fadeDuration = 1;
    [Space]
    [SerializeField] private UnityEvent OnShowStart = null;
    [SerializeField] private UnityEvent OnShowFinish = null;
    [SerializeField] private UnityEvent OnHideStart = null;
    [SerializeField] private UnityEvent OnHideFinish = null;

    private bool _isShowing;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
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