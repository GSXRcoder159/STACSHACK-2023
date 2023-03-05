using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigatorAI : MonoBehaviour
{
   public Waypoint startWaypoint;
   public Waypoint endWaypoint;
//    public NavigationAI navAI;
   public Dijkstra dijkstraObj;
   Waypoint currentTarget;
   
   AIController controller;
   List<Waypoint> route;

    void Start() {
        controller = this.gameObject.GetComponent<AIController>();
        // route = navAI.generatePath(startWaypoint, endWaypoint);
        Debug.Log("Start: " + startWaypoint.gameObject.name);
        Debug.Log("End: " + endWaypoint.gameObject.name);
        route = Dijkstra.dijkstra(startWaypoint, endWaypoint);
        // for (int i = 0; i < route.Count; i++) {
        //     Debug.Log(route[i].gameObject.name);
        // }
        currentTarget = route[0];
        controller.setTarget(currentTarget.transform);
    }
    
    void Update()
    {
        Debug.Log(currentTarget.gameObject.name);
        if (controller.isFinished()) {
            if (currentTarget == endWaypoint) {
                // End of route
                Debug.Log("Victory");
                controller.actor.SetSpeed(0);
                controller.actor.SetRotation(0);
            }
            route.Remove(currentTarget);
            if (route.Count == 0) {
                startWaypoint = currentTarget;
                endWaypoint = pickRandomTarget();
                // route = navAI.generatePath(startWaypoint, endWaypoint);
                route = Dijkstra.dijkstra(startWaypoint, endWaypoint);
            }
            else {
                currentTarget = route[0];
            }
            
            controller.setTarget(currentTarget.transform);
        }
    }

    Waypoint pickRandomTarget() {
        Waypoint[] waypoints = GameObject.FindObjectsOfType<Waypoint>();
        return waypoints[Random.Range(0, waypoints.Length)];
    }
}
