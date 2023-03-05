using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NavSpawner : MonoBehaviour
{

    public Waypoint startWaypoint;
    public Waypoint endWaypoint;
    public int count = 20;
    public GameObject carPrefab;
    public List<GameObject> cars;
    int genTime = 60;
    float startTime = 0;
    public int generation = 1;

    public TMPro.TextMeshProUGUI generationText;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < count; i++) {
            // Debug.Log("Instantiate" + i);
            GameObject car = Instantiate(carPrefab, startWaypoint.transform.position, startWaypoint.transform.rotation);
            WaypointNavigator waypointNavigator = car.GetComponent<WaypointNavigator>();
            NavigationAI ai = car.GetComponent<NavigationAI>();
            ai.loadActionWeights();

            waypointNavigator.navAI = ai;
            waypointNavigator.startWaypoint = startWaypoint;
            waypointNavigator.endWaypoint = endWaypoint;

            float weightSum = 0f;
            for (int j = 0; j < ai.actions.Count; j++) {
                float weight = Random.Range(0f, 1f);
                weightSum += weight;
                ai.actions[j] = (ai.actions[j].Item1, weight);
            }

            for (int j = 0; j < ai.actions.Count; j++) {
                ai.actions[j] = (ai.actions[j].Item1, ai.actions[j].Item2 / weightSum);
            }

            // Debug.Log("End " + i);
            cars.Add(car);
        }
        Time.timeScale = 5;
        generationText.text = "Generation: " + generation;
    }

    GameObject GeneSwap(NavigationAI parent1, NavigationAI parent2) {
        GameObject car = Instantiate(carPrefab, startWaypoint.transform.position, startWaypoint.transform.rotation);
        WaypointNavigator waypointNavigator = car.GetComponent<WaypointNavigator>();
        NavigationAI ai = car.GetComponent<NavigationAI>();
        ai.loadActionWeights();

        waypointNavigator.navAI = ai;
        waypointNavigator.startWaypoint = startWaypoint;
        waypointNavigator.endWaypoint = endWaypoint;


        for (int j = 0; j < ai.actions.Count; j++) {
            float newWeight = (parent1.actions[j].Item2 + parent2.actions[j].Item2) / 2.0f;
            ai.actions[j] = (ai.actions[j].Item1, newWeight);
        }


        return car;
    }

    void Breed() {
        startTime = Time.realtimeSinceStartup;
        List<GameObject> completedCars = new List<GameObject>();
        List<GameObject> uncompletedCars = new List<GameObject>();
        foreach (GameObject car in cars) {
            if (car.GetComponent<NavigationAI>().fitness < 0) {
                completedCars.Add(car);
            }
            else {
                uncompletedCars.Add(car);
            }
            completedCars = completedCars.OrderBy(car => car.GetComponent<NavigationAI>().fitness).ToList();
            uncompletedCars = uncompletedCars.OrderBy(car => car.GetComponent<NavigationAI>().fitness).ToList();
        }



        List<GameObject> sortedCars = completedCars;
        sortedCars.AddRange(uncompletedCars);
        int half = (int)(sortedCars.Count / 2.0f);
        cars.Clear();
        for (int i = 0; i < half; i++) {
            cars.Add(GeneSwap(sortedCars[i].GetComponent<NavigationAI>(), sortedCars[sortedCars.Count - i - 1].GetComponent<NavigationAI>()));
            cars.Add(GeneSwap(sortedCars[sortedCars.Count - i - 1].GetComponent<NavigationAI>(), sortedCars[i].GetComponent<NavigationAI>()));
        }

        Debug.Log(sortedCars.Count);
        for (int i = 0; i < sortedCars.Count; i++) {
            Destroy(sortedCars[i]);
        }
        Debug.Log(sortedCars[0]);

        generation++;
        generationText.text = "Generation: " + generation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - startTime > genTime) {
            Breed();
        }
    }
}
