using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Waypoint : MonoBehaviour
{
    

    [SerializeField]
    public List<WaypointInfo> waypoints;
    void Awake()
    {
        waypoints = new List<WaypointInfo>();
    }

    void Update()
    {
        
    }
    
    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if (waypoints != null) {
            foreach (WaypointInfo waypointInfo in waypoints) {
                if (waypointInfo.waypoint != null) {
                    Gizmos.DrawLine(this.transform.position, waypointInfo.waypoint.gameObject.transform.position);
                }
            }
        }
        
    }
}
