using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Lane nextLane;
    public Waypoint firstWaypoint;
    private Waypoint lastWaypoint;
    void Awake()
    {
        if (nextLane != null) {
            lastWaypoint = null;
            foreach (Transform child in this.GetComponentInChildren<Transform>()) {
                Waypoint waypoint = child.gameObject.GetComponent<Waypoint>();;
                if (waypoint.waypoints.Count == 0) {
                    lastWaypoint = waypoint;
                }
            }
            lastWaypoint.waypoints.Add(new WaypointInfo(nextLane.firstWaypoint, 1));
        }   
        
    }
}
