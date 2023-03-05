using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarSpawn : MonoBehaviour
{
    public int count = 20;
    public GameObject carPrefab;
    public Rigidbody actorPrefab;
    public List<GameObject> cars;
    public Transform target;
    int genTime = 20;
    float startTime = 0;
    public int generation = 1;

    public TMPro.TextMeshProUGUI generationText;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++) {
            GameObject car = Instantiate(carPrefab, transform.position, transform.rotation);
            Actor actor = car.GetComponent<Actor>();
            actor.actor = actorPrefab;
            AIController ai = car.GetComponent<AIController>();
            ai.targetTransform = target;
            ai.dSpeed = Random.Range(0f, ai.actor.maxSpeed);
            ai.dReverseSpeed = Random.Range(ai.actor.minSpeed, 0f);
            ai.dRotation = Random.Range(0f, ai.actor.maxTurnSpeed);
            ai.stoppingDistance = Random.Range(0f, 100f);
            ai.stoppingSpeed = Random.Range(0f, ai.actor.maxSpeed);
            ai.reverseTurnDistance = Random.Range(0f, 50f);
            ai.stoppingSpeedLimit = Random.Range(0f, ai.actor.maxSpeed);
            cars.Add(car);
        }
        Time.timeScale = 5;
        generationText.text = "Generation: " + generation;
    }

    GameObject GeneSwap(AIController parent1, AIController parent2) {
        GameObject car = Instantiate(carPrefab, this.transform.position, this.transform.rotation);
        AIController ai = car.GetComponent<AIController>();
        ai.targetTransform = target;

        ai.dSpeed = (parent1.dSpeed + parent2.dSpeed) / 2.0f;
        ai.dReverseSpeed = (parent1.dReverseSpeed + parent2.dReverseSpeed) / 2.0f;
        ai.dRotation = (parent1.dRotation + parent2.dRotation) / 2.0f;
        ai.stoppingDistance = (parent1.stoppingDistance + parent2.stoppingDistance) / 2.0f;
        ai.stoppingSpeed = (parent1.stoppingSpeed + parent2.stoppingSpeed) / 2.0f;
        ai.reverseTurnDistance = (parent1.reverseTurnDistance + parent2.reverseTurnDistance) / 2.0f;
        ai.stoppingSpeedLimit = (parent1.stoppingSpeedLimit + parent2.stoppingSpeedLimit) / 2.0f;
        
        return car;
    }

    void Breed() {
        startTime = Time.realtimeSinceStartup;
        List<GameObject> sortedCars = cars.OrderBy(car => car.GetComponent<AIController>().fitness).ToList();
        int half = (int)(sortedCars.Count / 2.0f);
        cars.Clear();
        for (int i = 0; i < half; i++) {
            cars.Add(GeneSwap(sortedCars[i].GetComponent<AIController>(), sortedCars[sortedCars.Count - i - 1].GetComponent<AIController>()));
            cars.Add(GeneSwap(sortedCars[sortedCars.Count - i - 1].GetComponent<AIController>(), sortedCars[i].GetComponent<AIController>()));
        }

        for (int i = 0; i < sortedCars.Count; i++) {
            Destroy(sortedCars[i]);
        }
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
