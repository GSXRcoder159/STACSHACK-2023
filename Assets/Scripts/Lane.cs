using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lane : MonoBehaviour
{
    public Waypoint nextWaypoint;
    private Waypoint lastWaypoint;
    void Awake()
    {
        if (nextWaypoint != null) {
            lastWaypoint = null;
            foreach (Transform child in this.GetComponentInChildren<Transform>()) {
                Waypoint waypoint = child.gameObject.GetComponent<Waypoint>();;
                if (waypoint.waypoints.Count == 0) {
                    lastWaypoint = waypoint;
                }
            }
            if (lastWaypoint != null) {
                lastWaypoint.waypoints.Add(new WaypointInfo(nextWaypoint, 0));
            }
            
        }   
        
    }
}
