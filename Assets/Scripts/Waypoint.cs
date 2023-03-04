using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Waypoint : MonoBehaviour
{
    

    [SerializeField]
    public List<WaypointInfo> waypoints;
    void Start()
    {
        foreach (WaypointInfo waypointInfo in waypoints) {
            if (waypointInfo.weight == 0) {
                waypointInfo.weight = WeightCalc.CalculateWeight(this, waypointInfo.waypoint);
            }
        }
    }

    public Lane lane () {
        return this.transform.parent.GetComponent<Lane>();
    }

    public GameObject roadSegment() {
        return this.transform.parent.parent.gameObject; 
    }
    
    void OnDrawGizmos() {
        
        if (waypoints != null) {
            foreach (WaypointInfo waypointInfo in waypoints) {
                Waypoint waypoint = waypointInfo.waypoint;
                if (waypoint != null) {
                    //If the connection is between segments
                    if (this.roadSegment() != waypoint.roadSegment()) {
                        Gizmos.color = Color.red;
                    }
                    //if the connection is between lanes
                    else if (this.lane() == waypoint.lane()) {
                        Gizmos.color = Color.green;
                    }
                    //if the connection is in the same lane
                    else {
                        Gizmos.color = Color.yellow;
                    }
                    Gizmos.DrawLine(this.transform.position, waypointInfo.waypoint.gameObject.transform.position);
                }
            }
        }
        
    }

}
