using System;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMaster : MonoBehaviour
{
    [Header("Button Master")]
    [SerializeField] private ButtonMasterSO _data = null;
    [SerializeField] private BUTTONMASTER_STATE _currentState = BUTTONMASTER_STATE.Food;
    [Space]
    [SerializeField] private Button _skipBtn = null;
    [SerializeField] private Button _resetBtn = null;
    [Space]
    [SerializeField] private Image _image = null;

    [Header("Animation")]
    [SerializeField] private Animation _animationButtonMaster = null;
    [SerializeField] private Animation _animationFoodSelection = null;

    private Button _button;
    private bool _isActive;

    private StatePauseHPEvent _statePauseHPEvent;
    private StatePauseSimpleEvent _statePauseSimpleEvent;
    private StateRunningEvent _stateRunningEvent;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _statePauseHPEvent = new StatePauseHPEvent();
        _statePauseSimpleEvent = new StatePauseSimpleEvent();
        _stateRunningEvent = new StateRunningEvent();
    }

    private void Start()
    {
        _image.sprite = _data.GetIcon(_currentState);
        _button.onClick.AddListener(Execute);
        
        // TODO Mariano: Add actions to buttons
        // _skipBtn.onClick.AddListener(Execute);
        // _resetBtn.onClick.AddListener(Execute);
    }

    private void OnEnable()
    {
        EventController.AddListener<StateInitialEvent>(OnStateInitialEvent);
        EventController.AddListener<StateRunningEvent>(OnStateRunningEvent);
        EventController.AddListener<StatePauseSimpleEvent>(OnStatePauseSimpleEvent);
        EventController.AddListener<StatePauseHPEvent>(OnStatePauseHPEvent);
        EventController.AddListener<StateStopEvent>(OnStateStopEvent);
        EventController.AddListener<StateSelectHPEvent>(OnStateSelectedHPEvent);
        EventController.AddListener<StateInfoEvent>(OnStateInfoEvent);
        EventController.AddListener<StateFullScreenEvent>(OnStateFullScreenEvent);
        EventController.AddListener<StateGraphEvent>(OnStateGraphEvent);
        EventController.AddListener<HightlightDataEvent>(OnHighlightData);
    }

    #region Event Handling

    private void OnStateInitialEvent(StateInitialEvent eventData)
    {
        throw new NotImplementedException();
    }

    private void OnStateRunningEvent(StateRunningEvent eventData)
    {
        ChangeState(BUTTONMASTER_STATE.Pause);
        _isActive = false;
    }

    private void OnStatePauseSimpleEvent(StatePauseSimpleEvent eventData)
    {
        ChangeState(BUTTONMASTER_STATE.Play);
    }

    private void OnStatePauseHPEvent(StatePauseHPEvent eventData)
    {
        ChangeState(BUTTONMASTER_STATE.Play);
    }

    private void OnStateStopEvent(StateStopEvent eventData)
    { }

    private void OnStateSelectedHPEvent(StateSelectHPEvent eventData)
    { }

    private void OnStateInfoEvent(StateInfoEvent eventData)
    { }

    private void OnStateFullScreenEvent(StateFullScreenEvent eventData)
    { }

    private void OnStateGraphEvent(StateGraphEvent eventData)
    { }
    private void OnHighlightData(HightlightDataEvent evt)
    {
        ChangeState(BUTTONMASTER_STATE.Cancel);
    }

    #endregion
    private void OnDisable()
    {
        EventController.RemoveListener<StateInitialEvent>(OnStateInitialEvent);
        EventController.RemoveListener<StateRunningEvent>(OnStateRunningEvent);
        EventController.RemoveListener<StatePauseSimpleEvent>(OnStatePauseSimpleEvent);
        EventController.RemoveListener<StatePauseHPEvent>(OnStatePauseHPEvent);
        EventController.RemoveListener<StateStopEvent>(OnStateStopEvent);
        EventController.RemoveListener<StateSelectHPEvent>(OnStateSelectedHPEvent);
        EventController.RemoveListener<StateInfoEvent>(OnStateInfoEvent);
        EventController.RemoveListener<StateFullScreenEvent>(OnStateFullScreenEvent);
        EventController.RemoveListener<StateGraphEvent>(OnStateGraphEvent);
        EventController.RemoveListener<HightlightDataEvent>(OnHighlightData);
    }

    public void ChangeState(BUTTONMASTER_STATE newState)
    {
        _currentState = newState;
        _image.sprite = _data.GetIcon(_currentState);
    }

    private void Execute()
    {
        _isActive = !_isActive;

        if (_isActive)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    [ContextMenu("Show")]
    public void Show()
    {
        switch (_currentState)
        {
            case BUTTONMASTER_STATE.Food:
                _animationFoodSelection.Show();
                _image.sprite = _data.GetIcon(BUTTONMASTER_STATE.Cancel);
                break;

            case BUTTONMASTER_STATE.Play:
                EventController.TriggerEvent(_stateRunningEvent);
                ChangeState(BUTTONMASTER_STATE.Pause);
                //_animationButtonMaster.Show();
                break;

            case BUTTONMASTER_STATE.Pause:
                EventController.TriggerEvent(_statePauseSimpleEvent);
                break;

            case BUTTONMASTER_STATE.Back:
                break;

            case BUTTONMASTER_STATE.Cancel:
                EventController.TriggerEvent(_stateRunningEvent);
                break;
        }

    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        switch (_currentState)
        {
            case BUTTONMASTER_STATE.Food:
                _animationFoodSelection.Hide();
                _image.sprite = _data.GetIcon(BUTTONMASTER_STATE.Food);
                break;

            case BUTTONMASTER_STATE.Play:
//                _animationButtonMaster.Hide();
                EventController.TriggerEvent(_stateRunningEvent);
                ChangeState(BUTTONMASTER_STATE.Pause);
                break;

            case BUTTONMASTER_STATE.Pause:
                EventController.TriggerEvent(_statePauseSimpleEvent);
                ChangeState(BUTTONMASTER_STATE.Play);
                break;

            case BUTTONMASTER_STATE.Back:
                break;

            case BUTTONMASTER_STATE.Cancel:
                break;
        }
    }

}