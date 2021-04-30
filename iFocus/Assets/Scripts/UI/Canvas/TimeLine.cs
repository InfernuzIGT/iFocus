using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeLine : MonoBehaviour
{
    public bool IsPlaying { get { return _isPlaying; } set { _isPlaying = value; } }
    public bool IsEndingModeOn { get { return _modeEnding; } set { _modeEnding = value; } }

    private bool _isAlreadyRunning = false;

    [SerializeField] private SettingsSO _settings;
    [SerializeField] private DiabetesTypes _type = DiabetesTypes.Normal;

    [SerializeField] private Image _imgTimeLineFill;
    [SerializeField] private GameObject _graphButton;

    /// <summary>
    /// Accordingly to their respective diabetes type.
    /// </summary>
    [SerializeField] private HighlightPointUISet[] _highlightPointSets;

    /// <summary>
    /// The total display time of the hole simulation if there where no stops on highlight points.
    /// </summary>
    [SerializeField] private int _currentHighlightPointIndex = 0;
    [SerializeField] private float _steps = 120;
    [SerializeField] private float _stepTime = 0.01f;
    [SerializeField] private float _deltaStep = 0.1f;
    [SerializeField] private bool _isPlaying = false;
    [SerializeField] private bool _isPaused = false;
    [SerializeField] private bool _stopOnReachHighlightPoint = true;
    [SerializeField] private TextMeshProUGUI _glucoseText;
    [SerializeField] private TextMeshProUGUI _insulineText;

    private Vector2[] _glucosePoints;
    private Vector2[] _insulinePoints;
    private Vector2[] _glucoseInsulineDisplayValues;
    private int discretice = 0;
    private float _currentStep = 0;
    private float _stepsToNextHP = 0;
    private float _stepCounter = 0;

    private CanvasGroupUtility _canvasUtility;

    private bool _modeEnding;

    private ModeEndingEvent _modeEndingEvent;

    private void OnEnable()
    {
        EventController.AddListener<StateRunningEvent>(OnStateRunningEvent);
        EventController.AddListener<StatePauseSimpleEvent>(OnStatePauseSimpleEvent);
    }

    #region Event Handling
    private void OnStatePauseSimpleEvent(StatePauseSimpleEvent eventData)
    {
        Pause();
    }

    private void OnStateRunningEvent(StateRunningEvent eventData)
    {
        if (_isAlreadyRunning)
        {
            //Restart();
            _isPaused = false;
            OnEndHighlightPointDataDisplay();
        }
        else
        {
            RunFromTheBeginning();
            _isAlreadyRunning = true;
        }
    }
    #endregion

    private void OnDisable()
    {
        EventController.RemoveListener<StateRunningEvent>(OnStateRunningEvent);
        EventController.RemoveListener<StatePauseSimpleEvent>(OnStatePauseSimpleEvent);
    }

    private void Awake()
    {
        _modeEndingEvent = new ModeEndingEvent();

        _stepsToNextHP = _steps / (_highlightPointSets[(int)_type]._highlightPoints.Length/* - 1*/);

        _canvasUtility = GetComponent<CanvasGroupUtility>();
        _canvasUtility.ShowInstant(false);
    }

    #region Event Listeners

    #endregion

    public void RunFromTheBeginning()
    {
        IsPlaying = true;
        ToggleVisibility(true);
        StartCoroutine(Running(0));
    }

    /// <summary>
    /// Use this to restart the timeline if the stop of the simulation was made by a forced HP selection.
    /// </summary>
    [ContextMenu("Restart")]
    public void Restart()
    {
        StopAllCoroutines();
        IsPlaying = true;
        //_graphButton.SetActive(false);
        StartCoroutine(Running(_currentHighlightPointIndex));
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        StopAllCoroutines();
        IsPlaying = false;
        _currentHighlightPointIndex = 0;
    }

    [ContextMenu("Pause")]
    public void Pause()
    {
        _isPaused = true;
    }

    [ContextMenu("Playu")]
    public void Play()
    {
        _isPaused = false;
    }

    /// <summary>
    /// Use this event to restart the simulation afeter a HP selection.
    /// </summary>
    public void OnEndHighlightPointDataDisplay()
    {
        IsPlaying = true;

        //TODO: Bueno, hay quever como va a hacer marian para mostrar este graph, supongo que con el sistema de eventos 
        // va a ser mas que suficiente.
        //_graphButton.SetActive(false);
    }

    private IEnumerator Running()
    {
        yield return StartCoroutine(Running(0));
    }

    private IEnumerator Running(int startingHighlightPointIndex)
    {
        if (startingHighlightPointIndex == 0)
        {
            for (int i = 0; i < _highlightPointSets[(int)_type]._highlightPoints.Length; i++)
            {
                _highlightPointSets[(int)_type]._highlightPoints[i].IsSelected = false;
            }
        }

        int displayValueIndex = 0;

        _stepCounter = 0;
        _currentStep = startingHighlightPointIndex * _stepsToNextHP;
        _currentHighlightPointIndex = startingHighlightPointIndex;

        yield return StartCoroutine(NextHighlightPoint());

        if (_currentHighlightPointIndex == 1)
        {
            if (_stopOnReachHighlightPoint)
            {
                _isPlaying = false;
                while (!_isPlaying)
                {
                    yield return null;
                }
            }
        }

        SimulationManager._control.MakeCameraZoomOut();


        while (IsPlaying && _currentStep < _steps && !_modeEnding)
        {
            if (_currentHighlightPointIndex + 1 == _highlightPointSets[(int)_type]._highlightPoints.Length)
                SetEndingMode();
            
            if (_currentHighlightPointIndex >= _highlightPointSets[(int)_type]._highlightPoints.Length)
            {
                break;
            }

            yield return new WaitForSecondsRealtime(_stepTime);
            // _imgTimeLineFill.fillAmount = _currentStep / _steps;
            displayValueIndex += discretice;

            if (displayValueIndex <= _glucoseInsulineDisplayValues.Length - 1)
                SetTexts(_glucoseInsulineDisplayValues[displayValueIndex].x.ToString("F2"), _glucoseInsulineDisplayValues[displayValueIndex].y.ToString("F2"));

            _currentStep += _deltaStep;

            if (_stepCounter < _stepsToNextHP)
            {
                _stepCounter += _deltaStep;
            }
            else
            {
                _stepCounter = 0;

                yield return StartCoroutine(NextHighlightPoint());

                if (_stopOnReachHighlightPoint)
                {
                    _isPlaying = false;

                    yield return new WaitUntil(() => _isPlaying == true);
                    SimulationManager._control.MakeCameraZoomOut();
                }
            }

            while (_isPaused)
            {
                yield return null;
            }
        }
    }

    private void SetEndingMode()
    {
        _modeEnding = true;
        EventController.TriggerEvent(_modeEndingEvent);
        SimulationManager._control.ResetCamera();

        switch (_type)
        {
            case DiabetesTypes.Normal:
                _highlightPointSets[(int)_type].ToggleAll(true);
                //_highlightPointSets[(int)_type]._highlightPoints[_highlightPointSets[(int)_type]._highlightPoints.Length - 1].IsSelected = true;
                break;
            case DiabetesTypes.T1DM:
                _highlightPointSets[(int)_type].ToggleAll(true);
               // _highlightPointSets[(int)_type]._highlightPoints[_highlightPointSets[(int)_type]._highlightPoints.Length - 1].IsSelected = false;
                break;

            case DiabetesTypes.T2DM:
                _highlightPointSets[(int)_type].ToggleAll(true);
                //_highlightPointSets[(int)_type]._highlightPoints[_highlightPointSets[(int)_type]._highlightPoints.Length - 1].IsSelected = false;
                break;
            default:
                break;
        }
    }

    public IEnumerator NextHighlightPoint()
    {
        yield return StartCoroutine(UpdateHighlighPoint());

        if (_currentHighlightPointIndex <= _highlightPointSets[(int)_type]._highlightPoints.Length)
        {
            _currentHighlightPointIndex++;
        }
        else
        {
            _currentHighlightPointIndex = 0;
        }
    }
    public void SelectHighlightPoint(int highlightPointIndex)
    {
        StopAllCoroutines();
        IsPlaying = false;
        _currentHighlightPointIndex = highlightPointIndex;
        _imgTimeLineFill.fillAmount = (_currentHighlightPointIndex * _stepsToNextHP) / _steps;

        UpdateHighlighPoint();
    }

    public IEnumerator UpdateHighlighPoint()
    {
        for (int i = 0; i < _highlightPointSets[(int)_type]._highlightPoints.Length; i++)
        {
            if (i != _currentHighlightPointIndex)
                _highlightPointSets[(int)_type]._highlightPoints[i].IsSelected = false;
            else
            {
                // _highlightPointSets[(int)_type]._highlightPoints[i].IsSelected = true;
                yield return new WaitForSeconds(_settings._waitBeforeTransition);
                yield return SimulationManager._control?.MakeTransition(i);
                yield return new WaitForSeconds(_settings._waitAfterTransition);
                yield return SimulationManager._control?.MakeCameraZoomIn();
                yield return new WaitForSeconds(_settings._waitAfterZoomIn);
                _highlightPointSets[(int)_type]._highlightPoints[i].IsSelected = true;
                SimulationManager._control.SelectHP(i);
            }
        }
    }

    public void SelectAllHighlighPoint()
    {
        for (int i = 0; i < _highlightPointSets[(int)_type]._highlightPoints.Length; i++)
        {
            _highlightPointSets[(int)_type]._highlightPoints[i].IsSelected = true;
        }
    }

    private void SetTexts(string glucoseValue, string insulineValue)
    {
        _glucoseText.text = glucoseValue;
        _insulineText.text = insulineValue;
    }

    public void SetDiabetesType(DiabetesTypes newType)
    {
        _type = newType;
    }

    public void StoreValues(Vector2[] glucoseValues, Vector2[] insulineValues)
    {
        _glucosePoints = new Vector2[glucoseValues.Length];
        _glucosePoints = glucoseValues;

        _insulinePoints = new Vector2[insulineValues.Length];
        _insulinePoints = insulineValues;

        float totalSteps = _steps / _deltaStep;

        discretice = Mathf.RoundToInt(_glucosePoints.Length / totalSteps);

        _glucoseInsulineDisplayValues = new Vector2[(int)totalSteps];

        for (int i = 0; i < totalSteps; i += discretice)
        {
            _glucoseInsulineDisplayValues[i] = new Vector2(_glucosePoints[i].y, _insulinePoints[i].y);
        }

        for (int i = 0; i < _glucoseInsulineDisplayValues.Length; i++) { }

    }

    public void ToggleVisibility(bool visible)
    {
        _canvasUtility.ShowInstant(visible);
    }

    public void ToggleModeEnding()
    {
        if (_modeEnding)
        {
            _modeEnding = false;
            _highlightPointSets[(int)_type].ToggleAll(false);
            _imgTimeLineFill.fillAmount = 0;
            ToggleVisibility(false);
            _glucoseText.text = "-";
            _insulineText.text = "-";
        }
    }
}