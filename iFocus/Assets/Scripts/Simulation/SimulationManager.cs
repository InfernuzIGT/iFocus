using System;
using System.Collections;
using Events;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Simulation), typeof(FunctionDrawer))]
public class SimulationManager : MonoBehaviour
{
    public static SimulationManager _control;

    [SerializeField] private Simulation _eulerFunction;
    [SerializeField] private FunctionDrawer _functionDrawerForGlucose;
    [SerializeField] private FunctionDrawer _functionDrawerForInsuline;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private TimeLine _timeLine;
    //[SerializeField] private UI _UI;
    [SerializeField] private float _displayTotalTime = 4;
    [SerializeField] private bool _instantDraw;

    /// <summary>
    /// Are whe inside the simulation? The simulation window.
    /// </summary>
    [SerializeField] private bool _isOnSimulation;

    /// <summary>
    /// Percentage of the current progress on the simulation.
    /// </summary>
    private float _simulationProgressPercentage;

    /// <summary>
    /// Use this to stop the simulation when needed.
    /// </summary>
    private bool _isStoped;

    private Coroutine _coroutineRunning;
    private Coroutine _coroutineUpdateUI;
    private Coroutine _coroutineTransition;
    private SwitchEvent _switchEvent;

    private void OnEnable()
    {
        /*
        EventController.AddListener<ChangeButtonStateEvent>(OnChangeButtonState);
        EventController.AddListener<ResetSimulationEvent>(OnResetSimulation);
        */
        EventController.AddListener<StateRunningEvent>(OnStateRunningEvent);
    }

    

    private void OnDisable()
    {
        /*
        EventController.RemoveListener<ChangeButtonStateEvent>(OnChangeButtonState);
        EventController.RemoveListener<ResetSimulationEvent>(OnResetSimulation);
        */

        EventController.RemoveListener<StateRunningEvent>(OnStateRunningEvent);

    }

    #region Event Listeners

    private void OnStateRunningEvent(StateRunningEvent eventData)
    {
        StartSimulation();
    }


    #endregion



    #region Acces Functions

    private void Awake()
    {
        if (_control != null)
            Destroy(_control);
        else
            _control = this;

        _switchEvent = new SwitchEvent();
        _eulerFunction.ToggleSimulationType(false);
    }

    public void RestarTimeLine()
    {
        _timeLine.OnEndHighlightPointDataDisplay();
    }

    [ContextMenu("StartSimulation")]
    public void StartSimulation()
    {
        _eulerFunction.ToggleSimulationType(true);

        _eulerFunction.StartSimulation();

        StoreValuesOnDrawer();
        StoreValuesOnTimeLine();

        // TODO: Aca esta el tema, en vez de dibujar la funcion de manera paralela, tengo que crear metodos para acceder a cada indice y poder dibujarla a demanda desde el Timeline.
        DrawValues();

        //_UI.buttonGraphImg.gameObject.SetActive(false);
    }

    /// <summary>
    /// Makes a camera transition to a desired highlightpoint with a given transition time.
    /// </summary>
    /// <param name="highlightPointIndex">Desired highlight point</param>
    /// <param name="transitionTime">Transition time</param>
    public Coroutine MakeTransition(int highlightPointIndex)
    {
        //TODO: No seas tan asqueroso por favor.

        //_cameraController.Zoom(-25);
        //_cameraController.RunZoom(_initialZoomValue);

        if (_coroutineTransition != null)
            StopCoroutine(_coroutineTransition);

        _coroutineTransition = StartCoroutine(Transition(highlightPointIndex));
        return _coroutineTransition;
    }

    public Coroutine MakeCameraZoom(float zoomAmount)
    {
        return _cameraController.RunZoom(zoomAmount);
    }

    public Coroutine MakeCameraZoomOut()
    {
        return _cameraController.ZoomOut();
    }

    public Coroutine MakeCameraZoomIn()
    {
        return _cameraController.ZoomIn();
    }

    private IEnumerator Transition(int highlightPointIndex)
    {
        //_UI.buttonPlay.gameObject.SetActive(false);

        yield return _cameraController.PositionAndRorationTransition(_eulerFunction.GetCurrentSimulation().highlightPoints[highlightPointIndex].transform.position);

        //_UI.buttonGraphImg.gameObject.SetActive(true);
        //_eulerFunction.GetCurrentSimulation().highlightPoints[highlightPointIndex].Select();
    }

    public void SelectHP(int highlightPointIndex)
    {
        //_UI.buttonGraphImg.gameObject.SetActive(true);
        //_eulerFunction.GetCurrentSimulation().highlightPoints[highlightPointIndex].Select();
        _eulerFunction.GetCurrentSimulation().highlightPoints[highlightPointIndex].Highlight(true);
    }

    /// <summary>
    /// Stores the real simulation values on the Function Drawer for later usage.
    /// </summary>
    private void StoreValuesOnDrawer()
    {
        _functionDrawerForGlucose.StorePoints(_eulerFunction.GetGlucosePoints().ToArray());
        _functionDrawerForInsuline.StorePoints(_eulerFunction.GetInsulinePoints().ToArray());
    }

    private void DrawValues()
    {
        if (_instantDraw)
        {
            _functionDrawerForGlucose.DrawFunctionInmediate();
            _functionDrawerForInsuline.DrawFunctionInmediate();
        }
        else
        {
            _functionDrawerForGlucose.DrawFunction();
            _functionDrawerForInsuline.DrawFunction();
        }
    }

    #endregion

    private void OnChangeButtonState(ChangeButtonStateEvent evt)
    {
        /*
        switch (evt.state)
        {
            case ButtonState.Play:
                _isOnSimulation = !_isOnSimulation;
                if (_isOnSimulation)
                {
                    _timeLine.Play();
                }
                else
                {
                    // TODO: Revisar si hay que resetear  valores etc.
                    StopAllCoroutines();
                    _timeLine.Pause();
                    _timeLine.ToggleModeEnding();
                }
                break;
            case ButtonState.SelectFood:
                AddFood(evt.index);
                _isOnSimulation = true;
                StartSimulation();
                break;
            case ButtonState.DisplayFood:
                break;
            case ButtonState.Fullscreen:
                break;
            case ButtonState.Reset:
                _timeLine.Stop();
                _timeLine.ToggleModeEnding();
                _eulerFunction.ToggleSimulationType(false);
                break;
        }
        */
    }

    private void OnResetSimulation(ResetSimulationEvent evt)
    {
        Debug.Log($"<color=green><b> SIMULATION RESETED </b></color>");

        // TODO Gon: Resetear toda la simulation 
    }

    private void AddFood(int foodIndex)
    {
        switch (foodIndex)
        {
            case 0:
                // TODO: Agregar comidas aca
                //Debug.Log("Oli: " + foodIndex);
                break;
            case 1:
                Debug.Log("Oli: " + foodIndex);
                break;
            case 2:
                Debug.Log("Oli: " + foodIndex);
                break;
        }
    }

    public void SwitchEvent()
    {
        EventController.TriggerEvent(_switchEvent);
    }

    public void StoreValuesOnTimeLine()
    {
        _timeLine.StoreValues(GetGlucoseValues(), GetInsulineValues());
    }

    private Vector2[] GetGlucoseValues()
    {
        return _eulerFunction.GetGlucosePoints().ToArray();
    }

    private Vector2[] GetInsulineValues()
    {
        return _eulerFunction.GetInsulinePoints().ToArray();
    }

    public void ResetCamera()
    {
        _cameraController.Reset();
    }

    public bool IsModeEndingOn()
    {
        return _timeLine.IsEndingModeOn;
    }

    [ContextMenu("TestZoomCamera")]
    public void TestZoomCamera()
    {
        StartCoroutine(_cameraController.PinchZoom(90f));
    }

}