using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimulationTypes: MonoBehaviour
{
    /// <summary>
    /// The highlight points corresponding to this particular SimulationType.
    /// </summary>
    public HighlightPoint[] highlightPoints;

    /// <summary>
    /// Variables defining the insulin behavior.
    /// </summary>
    protected struct InsulinVariables
    {
        public float alpha;

        public float beta;
    }

    /// <summary>
    /// Variables defining the insulin behavior.
    /// </summary>
    protected InsulinVariables insulinVariables;

    /// <summary>
    /// Variables defining the glucose behavior.
    /// </summary>
    protected struct GlucoseVariables
    {
        public float gama;

        public float delta;
    }

    /// <summary>
    /// Variables defining the glucose behavior.
    /// </summary>
    protected GlucoseVariables glucoseVariables;

    /// <summary>
    /// Must sets the values of the diferent variables on the 3 cases.
    /// </summary>
    protected virtual void SetValues()
    {

    }

    public float[] GetInsulineValues()
    {
        float[] values = new float[2] { insulinVariables.alpha, insulinVariables.beta };
        return values;
    }

    public float[] GetGlucoseValues()
    {
        float[] values = new float[2] { glucoseVariables.gama, glucoseVariables.delta};
        return values;
    }
}
