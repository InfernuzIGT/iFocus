using System;
using System.Collections;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSideScreen : Animation
{
    [Header("Side Screen")]
    [SerializeField] private RectTransform _slideScreen = null;
    [Space]
    [SerializeField] private Button[] _simulationBtn = null;
    [Space]
    [SerializeField] private Button _quizBtn = null;
    [SerializeField] private Button _configurationBtn = null;
    [SerializeField] private Button _creditsBtn = null;

    private CanvasGroup _canvasGroup;

    public override void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        // TODO Mariano: Add listeners
        // _quizBtn.onClick.AddListener(() => OpenVideo(true));

        base.Start();
    }

    [ContextMenu("Show")]
    public override void Show()
    {
        base.Show();

        _canvasGroup.DOFade(1, _duration);
        _slideScreen.DOMoveX(0, _duration);
    }

    [ContextMenu("Hide")]
    public override void Hide()
    {
        base.Hide();

        _canvasGroup.DOFade(0, _duration);
        _slideScreen.DOMoveX(-376, _duration);
    }

    public override void SetInteraction(bool interactuable)
    {
        _canvasGroup.interactable = interactuable;
    }

}