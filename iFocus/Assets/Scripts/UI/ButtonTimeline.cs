using System;
using DG.DemiLib;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTimeline : MonoBehaviour
{
    [Header("Button Timeline")]
    [SerializeField] private int id = 0;
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private bool _isLocked = true;
    [SerializeField] private bool _imPathology;

    [Header("Materials")]
    [SerializeField, Range(0f, 3f)] private float _matDuration = 1;
    [SerializeField] private Material _matBody;
    [SerializeField] private Material _matCurrentOrgan;

    private HightlightDataEvent _hightlightDataEvent;
    private StatePauseHPEvent _statePauseHPEvent;
    private SwitchEvent _switchEvent;

    private Button _button;
    private CanvasGroup _canvasGroup;

    private int _hash_Lerp = Shader.PropertyToID("_Lerp");
    private int _hash_IsGreyScale = Shader.PropertyToID("_IsGreyscale");

    // Properties
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            _canvasGroup.interactable = _isSelected;
        }
    }

    public bool IsLocked { get { return _isLocked; } set { _isLocked = value; } }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _switchEvent = new SwitchEvent();
    }

    private void Start()
    {
        _hightlightDataEvent = new HightlightDataEvent();
        _hightlightDataEvent.id = id;

        _statePauseHPEvent = new StatePauseHPEvent();

        if (_imPathology)
        {
            _button.onClick.AddListener(SwitchEvent);
            return;
        }

        _button.onClick.AddListener(Select);
    }

    private void OnEnable()
    {
        EventController.AddListener<HightlightDataEvent>(OnHighlightDataEvent);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<HightlightDataEvent>(OnHighlightDataEvent);
    }

    public void SwitchEvent()
    {
        IsSelected = true;
        EventController.TriggerEvent(_switchEvent);
    }

    private void Select()
    {
        //IsSelected = true;
        EventController.TriggerEvent(_hightlightDataEvent);
    }

    private void OnHighlightDataEvent(HightlightDataEvent eventHandler)
    {
        if (eventHandler.id == id)
            IsSelected = true;
    }

    public void ShowOrgans(bool show, bool instant = false)
    {
        if (_matCurrentOrgan == null)return;

        if (instant)
        {
            _matCurrentOrgan.SetFloat(_hash_Lerp, show ? 1 : 0);
        }
        else
        {
            _matCurrentOrgan.DOFloat(show ? 1 : 0, _hash_Lerp, _matDuration);
        }
    }

    public void SelectOrgans(bool show)
    {
        if (_matCurrentOrgan == null)return;

        _matCurrentOrgan.SetFloat(_hash_IsGreyScale, show ? 0 : 1);
    }

    public void ShowBody(bool show, bool instant = false)
    {
        if (instant)
        {
            _matBody.SetFloat(_hash_Lerp, show ? 1 : 0);
        }
        else
        {
            _matBody.DOFloat(show ? 1 : 0, _hash_Lerp, _matDuration);
        }
    }

}