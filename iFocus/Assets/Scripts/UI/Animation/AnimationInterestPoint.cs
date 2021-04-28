using System;
using System.Collections;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AnimationInterestPoint : Animation
{
    [Header("Interest Point")]
    [SerializeField] private RectTransform _panelInformation = null;
    [SerializeField] private RawImage _videoImg = null;
    [SerializeField] private RawImage _Img = null;
    [Space]
    [SerializeField] private CanvasGroup _containerButtons = null;
    [SerializeField] private CanvasGroup _buttonBack = null;
    [Space]
    [SerializeField] private TextMeshProUGUI _titleTxt = null;
    [SerializeField] private TextMeshProUGUI _subtitleTxt = null;
    [Space]
    [SerializeField] private TextMeshProUGUI _descriptionTxt = null;
    [SerializeField] private RawImage _graphImg = null;
    [Space]
    [SerializeField] private Button _videoBtn = null;
    [SerializeField] private Button _backBtn = null;
    [SerializeField] private Button _graphBtn = null;
    [SerializeField] private Button _infoBtn = null;

    private CanvasGroup _canvasGroup;
    private VideoPlayer _videoPlayer;

    private bool _isGraph;

    private Vector2 _sizeOpen = new Vector2(0, 500);
    private Vector2 _sizeClose = new Vector2(0, 150);

    public override void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _videoBtn.onClick.AddListener(() => OpenVideo(true));

        _backBtn.onClick.AddListener(ClosePanel);
        _graphBtn.onClick.AddListener(() => OpenPanel(true));
        _infoBtn.onClick.AddListener(() => OpenPanel(false));

        // _videoPlayer.prepareCompleted += VideoSuccess;
        // _videoPlayer.errorReceived += VideoFail;

        base.Start();
    }

    private void OnEnable()
    {
        EventController.AddListener<HightlightDataEvent>(OnHighlightData);
        EventController.AddListener<StateRunningEvent>(OnStateRunningEvent);
    }

    #region Event Handling

    private void OnStateRunningEvent(StateRunningEvent eventData)
    {
        Hide();
    }

    #endregion
    private void OnDisable()
    {
        EventController.RemoveListener<HightlightDataEvent>(OnHighlightData);
        EventController.RemoveListener<StateRunningEvent>(OnStateRunningEvent);
    }

    // [ContextMenu("Show")]
    public override void Show()
    {
        base.Show();

        _canvasGroup.DOFade(1, _duration);

        // TODO Mariano: Animate InterestPointPreview
        // TODO Mariano: Animate Panel Information
    }

    // [ContextMenu("Hide")]
    public override void Hide()
    {
        base.Hide();

        _canvasGroup.DOFade(0, _duration);

        // TODO Mariano: Animate InterestPointPreview
        // TODO Mariano: Animate Panel Information
    }

    public override void SetInteraction(bool interactuable)
    {
        _canvasGroup.interactable = interactuable;
        _canvasGroup.blocksRaycasts = interactuable;
    }

    private void OpenVideo(bool isOpening)
    {
        Debug.Log($"<b> OPEN VIDEO: {isOpening} </b>");
    }

    private void OpenPanel(bool isGraph)
    {
        _isGraph = isGraph;

        if (_isGraph)
        {
            _graphImg.DOFade(1, _duration);
        }
        else
        {
            _descriptionTxt.DOFade(1, _duration);
        }

        _panelInformation.DOSizeDelta(_sizeOpen, _duration);

        _subtitleTxt.DOFade(0, _duration);

        _containerButtons.DOFade(0, _duration);
        _buttonBack.DOFade(1, _duration);

        _containerButtons.interactable = false;
        _containerButtons.blocksRaycasts = false;
        _buttonBack.interactable = true;
        _buttonBack.blocksRaycasts = true;
    }

    private void ClosePanel()
    {
        if (_isGraph)
        {
            _graphImg.DOFade(0, _duration);
        }
        else
        {
            _descriptionTxt.DOFade(0, _duration);
        }

        _panelInformation.DOSizeDelta(_sizeClose, _duration);

        _subtitleTxt.DOFade(1, _duration);

        _containerButtons.DOFade(1, _duration);
        _buttonBack.DOFade(0, _duration);

        _containerButtons.interactable = true;
        _containerButtons.blocksRaycasts = true;
        _buttonBack.interactable = false;
        _buttonBack.blocksRaycasts = false;
    }

    #region TODO Mariano : Review

    private void OnHighlightData(HightlightDataEvent evt)
    {
        //if (_videoPlayer.clip != null)
        //{
        //    // _videoPlayer.clip = _currentHighlightData.video;
        //    _videoPlayer.Prepare();
        //}

        Show();
    }

    private void VideoSuccess(VideoPlayer source)
    {
        Debug.Log($"<color=green><b>[VIDEO] </b></color> Prepare completed.");

        _videoImg.texture = _videoPlayer.texture;

        _videoPlayer.Play();
    }

    private void VideoFail(VideoPlayer source, string message)
    {
        Debug.Log($"<color=red><b>[VIDEO] </b></color> Error: {message}");
    }

    private void ChangeOrientation(bool isFull)
    {
        Screen.orientation = isFull ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
    }

    #endregion

}