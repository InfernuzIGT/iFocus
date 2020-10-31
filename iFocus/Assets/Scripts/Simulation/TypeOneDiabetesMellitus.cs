using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeOneDiabetesMellitus : SimulationTypes
{
    private void Awake()
    {
        SetValues();
    }

    protected override void SetValues()
    {
        insulinVariables.alpha = 0.916f;
        insulinVariables.beta = 0.198f;
        glucoseVariables.gama = 3.23f;
        glucoseVariables.delta = 3.04f;
    }
}
