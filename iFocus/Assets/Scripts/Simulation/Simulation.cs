using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    #region Simulation

    [Header("Assignments")]

    [SerializeField]
    DiabetesTypes currentDiabetesType;

    /// <summary>
    /// This is are all the 3 simulation types of the diabetes.
    /// </summary>
    [SerializeField]
    SimulationTypes[] simulationType;

    [Header("Values")]

    /// <summary>
    /// Current insulin values.
    /// </summary>
    [SerializeField]
    private float currentInsulin;

    /// <summary>
    /// Current glucose values.
    /// </summary>
    [SerializeField]
    private float currentGlucose;

    [Header("Simulation")]
    /// <summary>
    /// Variables controlling the simulation.
    /// </summary>
    [SerializeField]
    private Settings simulationSettings;

    /// <summary>
    /// Variables controlling the simulation.
    /// </summary>
    [System.Serializable]
    private struct Settings
    {
        /// <summary>
        /// Multiplicator velocity for the simulation.
        /// </summary>
        public int multiplier;

        /// <summary>
        /// Time elapsted bewteen calculated variables, also the simulation presition.
        /// </summary>
        public float step;

        /// <summary>
        /// The amount of time that 
        /// </summary>
        public float simulationTime;

        /// <summary>
        /// Is the amount of samples a discrete and fixed value or not.
        /// </summary>
        public bool discrete;
    }

    [SerializeField]
    private List<Vector2> glucosePoints = new List<Vector2>();

    [SerializeField]
    private List<Vector2> insulinePoints = new List<Vector2>();

    [Header("Debug")]

    [Range(0f, 10f)]
    public float insulinResistance = 1f;
    [Range(0f, 10f)]
    public float insulinDeficience = 1f;
    public float globalDeficience = 0f;

    #endregion

    private void OnEnable()
    {
        EventController.AddListener<SwitchHighlightPointEvent>(OnSwitchHighlightPoint);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<SwitchHighlightPointEvent>(OnSwitchHighlightPoint);
    }

    private void OnSwitchHighlightPoint(SwitchHighlightPointEvent evt)
    {
        for (int i = 0; i < simulationType[(int)currentDiabetesType].highlightPoints.Length; i++)
        {
            simulationType[(int)currentDiabetesType].highlightPoints[i].ChangeLock(evt.isLocked);
        }
        
        // Debug.Log ($"<b> Locked: {evt.isLocked} </b>");
    }

    #region Initialization & Unity CallBacks

    [ContextMenu("Reset")]
    private void ResetValues()
    {
        currentGlucose = 0;
        currentInsulin = 0;
        glucosePoints.Clear();
        insulinePoints.Clear();
    }

    #endregion

    #region Simulation Functions

    /// <summary>
    /// Calculates the glucose using the euler method.
    /// </summary>
    public float calculateNextGlucose(float[] glucoseGamaDelta)
    {
        currentGlucose = currentGlucose + simulationSettings.step * (-1 * glucoseGamaDelta[0] * (currentInsulin) * currentGlucose + glucoseGamaDelta[1] * currentGlucose);
        return currentGlucose;
    }

    /// <summary>
    /// Calculates the insulin using the euler method.
    /// </summary>
    public float calculateNextInsulin(float[] insulineAlphaBeta)
    {
        currentInsulin = currentInsulin + simulationSettings.step * ((-1 * insulineAlphaBeta[0] * (currentInsulin / insulinDeficience) + insulineAlphaBeta[1] * currentGlucose) * insulinResistance) - globalDeficience;
        return currentInsulin;
    }

    public void Simulate()
    {
        float elapsedTime = 0;

        while (elapsedTime < simulationSettings.simulationTime)
        {
            insulinePoints.Add(new Vector2(elapsedTime, calculateNextInsulin(simulationType[(int)currentDiabetesType].GetInsulineValues())));
            glucosePoints.Add(new Vector2(elapsedTime, calculateNextGlucose(simulationType[(int)currentDiabetesType].GetGlucoseValues())));
            elapsedTime += Time.deltaTime;
        }
    }

    public void DiscreteSimulation()
    {
        float sample = 0;
        float samples = 1200;

        while (sample < samples)
        {
            // When multiplying by deltaTime you ensure that the value of the first component of the position vector stays in place.

            insulinePoints.Add(new Vector2(sample * Time.deltaTime, calculateNextInsulin(simulationType[(int)currentDiabetesType].GetInsulineValues())));
            glucosePoints.Add(new Vector2(sample * Time.deltaTime, calculateNextGlucose(simulationType[(int)currentDiabetesType].GetGlucoseValues())));
            sample++;
        }
    }

    /// <summary>
    /// Coroutine of the simulation.
    /// </summary>
    public IEnumerator SimulationRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < simulationSettings.simulationTime)
        {
            for (int i = 0; i < simulationSettings.multiplier; i++)
            {
                insulinePoints.Add(new Vector2(elapsedTime, calculateNextInsulin(simulationType[(int)currentDiabetesType].GetInsulineValues())));
                glucosePoints.Add(new Vector2(elapsedTime, calculateNextGlucose(simulationType[(int)currentDiabetesType].GetGlucoseValues())));
            }

            elapsedTime += Time.deltaTime;

            yield return new WaitForSeconds(simulationSettings.step);
        }
    }

    #endregion

    #region Access Functions

    /// <summary>
    /// Returns the current simulation.
    /// </summary>
    /// <returns></returns>
    public SimulationTypes GetCurrentSimulation()
    {
        return simulationType[(int)currentDiabetesType];
    }

    public SimulationTypes GetSimulationByType(DiabetesTypes type)
    {
        return simulationType[(int)type];
    }

    public void ToggleSimulationType(bool isActive)
    {
        simulationType[(int)currentDiabetesType].gameObject.SetActive(isActive);
    }

    public void SetType(Int32 type)
    {
        SetType((DiabetesTypes)type);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < GetSimulationByType((DiabetesTypes)i).highlightPoints.Length; j++)
            {
                GetSimulationByType((DiabetesTypes)i).highlightPoints[j].gameObject.SetActive(false);
            }

        }

        for (int i = 0; i < GetCurrentSimulation().highlightPoints.Length; i++)
        {
            GetCurrentSimulation().highlightPoints[i].gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// Sets the current diabetes type.
    /// </summary>
    /// <param name="type"></param>
    private void SetType(DiabetesTypes type)
    {
        currentDiabetesType = type;
    }

    public List<Vector2> GetGlucosePoints()
    {
        return glucosePoints;
    }

    public List<Vector2> GetInsulinePoints()
    {
        return insulinePoints;
    }

    #endregion

    #region ContextMenu Functions

    /// <summary>
    /// Starts the simulation.
    /// </summary>
    [ContextMenu("Start Simulation")]
    public void StartSimulation()
    {
        ResetValues();

        AddGlucose();

        if (simulationSettings.discrete)
            DiscreteSimulation();
        else
            Simulate();
    }
    /// <summary>
    /// Adds the glucose to the body.
    /// </summary>
    [ContextMenu("Add Glucose")]
    public void AddGlucose()
    {
        AddGlucose(1f);
    }

    public void AddGlucose(float amount)
    {
        currentGlucose += amount;
    }

    #endregion
}