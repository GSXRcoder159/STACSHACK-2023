using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeightCalcSetup : MonoBehaviour
{
    public float laneBiasPercentage;
    void Awake () {
        WeightCalc.laneBias = 1.0f + (0.01f * laneBiasPercentage);
    }
}
