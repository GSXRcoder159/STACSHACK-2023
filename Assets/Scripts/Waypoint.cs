using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Waypoint : MonoBehaviour
{
    

    [SerializeField]
    public List<WaypointInfo> waypoints;
    void Update()
    {
        
    }
    
    void OnDrawGizmos() {
        
        if (waypoints != null) {
            foreach (WaypointInfo waypointInfo in waypoints) {
                Waypoint waypoint = waypointInfo.waypoint;
                if (waypoint != null) {
                    if (this.transform.parent.parent != waypoint.transform.parent.parent) {
                        Gizmos.color = Color.red;
                    }
                    else if (this.transform.parent == waypoint.transform.parent) {
                        Gizmos.color = Color.green;
                    }
                    else {
                        Gizmos.color = Color.yellow;
                    }
                    Gizmos.DrawLine(this.transform.position, waypointInfo.waypoint.gameObject.transform.position);
                }
            }
        }
        
    }

}
