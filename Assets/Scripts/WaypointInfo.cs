using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class WaypointInfo
{
    [SerializeField]
    public Waypoint waypoint;
    [SerializeField]
    public int weight;


    public WaypointInfo(Waypoint _waypoint, int _weight) {
        waypoint = _waypoint;
        weight = _weight;
    }
}
