using Events;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTimeline : MonoBehaviour
{
    [Header("Button Timeline")]
    [SerializeField] private int id = 0;
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private bool _isLocked = true;
    [SerializeField] private bool _imPathology;

    private HightlightDataEvent _hightlightDataEvent;
    private StatePauseHPEvent _statePauseHPEvent;
    private SwitchEvent _switchEvent;

    private Button _button;
    private CanvasGroup _canvasGroup;


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
}