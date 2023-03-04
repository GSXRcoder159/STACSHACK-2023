using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class WaypointInfo
{
    [SerializeField]
    public Waypoint waypoint;
    [SerializeField]
    public float weight;


    public WaypointInfo(Waypoint _waypoint, float _weight) {
        waypoint = _waypoint;
        weight = _weight;
    }
}
