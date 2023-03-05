using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dijkstra : MonoBehaviour
{

    static List<Waypoint> getPath(Waypoint current, List<Tuple<Waypoint, Waypoint>> parents, List<Waypoint> path) {
        if (current == null) {
            return path;
        }
        path.Add(current);
        return getPath(parents.Find(x => x.Item1 == current).Item2, parents, path);
    }
    public static List<Waypoint> dijkstra(Waypoint startPoint, Waypoint endPoint)
    {
        List<Tuple<Waypoint, float>> distance = new List<Tuple<Waypoint, float>>();
        List<Waypoint> visited = new List<Waypoint>(); 
        List<Waypoint> unvisited = new List<Waypoint>();
        List<Waypoint> path = new List<Waypoint>();
        List<Tuple<Waypoint, Waypoint>> parents = new List<Tuple<Waypoint, Waypoint>>();
        Waypoint current = startPoint;

        foreach (WaypointInfo waypointInfo in startPoint.waypoints) {
            distance.Add(new Tuple<Waypoint, float>(waypointInfo.waypoint, float.MaxValue));
            unvisited.Add(waypointInfo.waypoint);
        }
        distance.Add(new Tuple<Waypoint, float>(startPoint, 0));
        parents.Add(new Tuple<Waypoint, Waypoint>(startPoint, null));

        while (current != endPoint) {
            foreach (WaypointInfo waypointInfo in current.waypoints) {
                if (!visited.Contains(waypointInfo.waypoint)) {
                    // add a distance entry
                    if (!distance.Contains(new Tuple<Waypoint, float>(waypointInfo.waypoint, float.MaxValue))) {
                        distance.Add(new Tuple<Waypoint, float>(waypointInfo.waypoint, float.MaxValue));
                    }
                    if (!unvisited.Contains(waypointInfo.waypoint) ) { //&& waypointInfo.waypoint.active
                        unvisited.Add(waypointInfo.waypoint);
                    }
                    parents.Add(new Tuple<Waypoint, Waypoint>(waypointInfo.waypoint, current));
                }
            }
            visited.Add(current);
            unvisited.Remove(current);

            // update distances of unvisited children
            foreach (WaypointInfo waypointInfo in current.waypoints) {
                if (!visited.Contains(waypointInfo.waypoint)) {
                    if (distance.Find(x => x.Item1 == current).Item2 + waypointInfo.weight < distance.Find(x => x.Item1 == waypointInfo.waypoint).Item2) {
                        distance[distance.FindIndex(x => x.Item1 == waypointInfo.waypoint)] = new Tuple<Waypoint, float>(waypointInfo.waypoint, distance.Find(x => x.Item1 == current).Item2 + waypointInfo.weight);
                    }
                }
            }

            if (unvisited.Count == 0) {
                Debug.Log("no path found");
                return null;
            }

            current = unvisited[0];
            foreach (Waypoint waypoint in unvisited) {
                if (distance.Find(x => x.Item1 == waypoint).Item2 < distance.Find(x => x.Item1 == current).Item2) {
                    current = waypoint;
                }
            }

            if (current == endPoint) {
                Debug.Log("path found");
                // break;
            }
        }
        List<Waypoint> path2 = getPath(current, parents, path);
        path2.Reverse();
        return path2;
    }


    // test method
    public void testDijkstra() {
        Waypoint[] waypointsAll = FindObjectsOfType<Waypoint>();
        Waypoint[] waypoints = new Waypoint[6];
        for (int i = 0; i < (waypointsAll.Length/2); i++) {
            waypoints[i] = waypointsAll[i];
        }
        Waypoint startPoint = waypoints[4];
        Waypoint endPoint = waypoints[0];
        List<Waypoint> path = dijkstra(startPoint, endPoint);
        foreach (Waypoint waypoint in path) {
            Debug.Log(waypoint.gameObject.name);
        }
    }
}