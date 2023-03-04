using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightCalc {
    public static float laneBias;

    public static float CalculateWeight (Waypoint fromWaypoint, Waypoint toWaypoint) {
        Vector3 from = fromWaypoint.transform.position;
        Vector3 to = toWaypoint.transform.position;

        float pureDist = Vector3.Distance(from, to);

        //Bias against changing lanes
        float laneChangeBias = fromWaypoint.lane() != toWaypoint.lane() ? laneBias : 1.0f;

        


        return pureDist * laneChangeBias;
    }
}
 