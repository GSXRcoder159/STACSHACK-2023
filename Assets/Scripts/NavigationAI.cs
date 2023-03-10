using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NavigationAI : MonoBehaviour
{
    public float randomWeight = 0.2f;
    public float distanceGreedyWeight = 0.2f;
    public float efficiencyGreedyWeight = 0.2f;
    public float alwaysFirstWeight = 0.2f;
    public float mostOptionsWeight = 0.2f;
    public int maxPathLength = 20;
    public float fitness;
    public Waypoint startWaypoint;
    public Waypoint targetWaypoint;

    public List<(Func<Waypoint, Waypoint, Waypoint>, float)> actions;
    
    void Start() {
        loadActionWeights();
    }

    public List<Waypoint> generatePath(Waypoint startWaypoint, Waypoint targetWaypoint) {
        List<Waypoint> path = new List<Waypoint>();
        float weightSum = 0f;
        Waypoint nextWaypoint = chooseNextWaypoint(startWaypoint, targetWaypoint);
        path.Add(nextWaypoint);
        weightSum += startWaypoint.waypoints.Find(waypointInf => waypointInf.waypoint == nextWaypoint).weight;
        
        while (path.Count < maxPathLength && path[path.Count - 1] != targetWaypoint) {
            nextWaypoint = chooseNextWaypoint(path[path.Count - 1], targetWaypoint);
            
            weightSum += path[path.Count - 1].waypoints.Find(waypointInf => waypointInf.waypoint == nextWaypoint).weight;
            path.Add(nextWaypoint);

        }
        setFitness(path, targetWaypoint, weightSum);
        return path;

    }
    public Waypoint chooseNextWaypoint (Waypoint currentWaypoint, Waypoint targetWaypoint) {
        float choice = UnityEngine.Random.Range(0, 1);
        Func<Waypoint, Waypoint, Waypoint> chosenAction = null;

        foreach (ValueTuple<Func<Waypoint, Waypoint, Waypoint>, float> tuple in actions) {
            Func<Waypoint, Waypoint, Waypoint> action = tuple.Item1;
            float weight = tuple.Item2;

            choice -= weight;
            if (choice <= 0f) {
                chosenAction = action;
                break;
            }
        }

        return chosenAction.Invoke(currentWaypoint, targetWaypoint);
    }
    public void loadActionWeights() {
        actions = new List<(Func<Waypoint, Waypoint, Waypoint>, float)>{
            (randomWaypoint, randomWeight), 
            (distanceGreedyWaypoint, distanceGreedyWeight), 
            (efficiencyGreedyWaypoint, efficiencyGreedyWeight), 
            (alwaysFirstWaypoint, alwaysFirstWeight),
            (mostOptionsWaypoint, mostOptionsWeight)
            };
    }
    public Waypoint randomWaypoint(Waypoint currentWaypoint, Waypoint targetWaypoint) {
        int index = UnityEngine.Random.Range(0, currentWaypoint.waypoints.Count);
        return currentWaypoint.waypoints[index].waypoint;
    }

    public Waypoint distanceGreedyWaypoint(Waypoint currentWaypoint, Waypoint targetWaypoint) {
        Waypoint closest = null;
        float closestDist = Mathf.Infinity;
        foreach (WaypointInfo waypointInfo in currentWaypoint.waypoints) {
            Waypoint waypoint = waypointInfo.waypoint;
            float dist = Vector3.Distance(waypoint.transform.position, targetWaypoint.transform.position);
            if (dist < closestDist) {
                closestDist = dist;
                closest = waypoint;
            }
        }

        return closest;
    }

    public Waypoint efficiencyGreedyWaypoint(Waypoint currentWaypoint, Waypoint targetWaypoint) {
        Waypoint best = null;
        float currentDist = Vector3.Distance(currentWaypoint.transform.position, targetWaypoint.transform.position);
        float greatestGainRatio = 0f;
        foreach (WaypointInfo waypointInfo in currentWaypoint.waypoints) {
            Waypoint waypoint = waypointInfo.waypoint;
            float weight = waypointInfo.weight;
            float gainRatio = (Vector3.Distance(waypoint.transform.position, targetWaypoint.transform.position) - currentDist) / weight;
            if (gainRatio > greatestGainRatio) {
                greatestGainRatio = gainRatio;
                best = waypoint;
            }
        }

        return best;
    }

    public Waypoint alwaysFirstWaypoint(Waypoint currentWaypoint, Waypoint targetWaypoint) {
        return currentWaypoint.waypoints[0].waypoint;
    }

    public Waypoint mostOptionsWaypoint(Waypoint currentWaypoint, Waypoint targetWaypoint) {
        int highestOptions = 0;
        Waypoint best = null;
        foreach (WaypointInfo waypointInfo in currentWaypoint.waypoints) {
            Waypoint waypoint = waypointInfo.waypoint;
            int options = waypointInfo.waypoint.waypoints.Count;
            
            if (options > highestOptions) {
                highestOptions = options;
                best = waypoint;
            }
        }
        return best;
    }

    public void setFitness(List<Waypoint> path, Waypoint targetWaypoint, float totalWeight) {
        if (path[path.Count - 1] == targetWaypoint) {
            fitness = -totalWeight;
        }
        else {
            fitness = Vector3.Distance(path[path.Count - 1].transform.position, targetWaypoint.transform.position);
        }
    }
}
