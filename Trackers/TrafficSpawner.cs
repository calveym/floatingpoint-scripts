using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour {

    PopulationManager populationManager;
    public static TrafficSpawner instance;  // singleton instance

    List<GameObject> cars;  // List of all cars in scene
    bool spawnCars;  // Controls car spawning

    [Tooltip("If checked, car spawn check will emanate from target")]
    public bool useTarget;
    [Tooltip("Alternate spawn check location")]
    public GameObject target;

    public GameObject carA;
    public GameObject carB;
    public GameObject carC;
    List<GameObject> carList;  // List of spawnable cars

    private void Awake()
    {
        if (instance != this)
            instance = this;
        cars = new List<GameObject>();
        spawnCars = true;

        carList = new List<GameObject>();
        carList.Add(carA);
        carList.Add(carB);
        carList.Add(carC);
    }

    private void Start()
    {
        StartCoroutine("CheckNumCars");
    }

    void SpawnCars(int numToSpawn)
    {
        List<GameObject> spawnLocations = GetSpawnLocations();
        int side = Random.Range(0, 1);
        if (spawnLocations.Count < numToSpawn)
        {
            for(int i = 0; i < spawnLocations.Count; i++)
            {
                if (side == 0)
                    SpawnCar(spawnLocations[i].transform.Find("StraightLeftStart").gameObject);
                else
                    SpawnCar(spawnLocations[i].transform.Find("StraightRightStart").gameObject);
            }
        }
        else if(spawnLocations.Count >= numToSpawn)
        {
            for(int i = 0; i < numToSpawn; i++)
            {
                if(side == 0)
                    SpawnCar(spawnLocations[i].transform.Find("StraightLeftStart").gameObject);
                else
                    SpawnCar(spawnLocations[i].transform.Find("StraightRightStart").gameObject);
            }
        }
    }

    public void RemoveCar(GameObject car)
    {
        if(cars.Contains(car))
        {
            cars.Remove(car);
            Destroy(car);
        }
    }

    List<GameObject> GetSpawnLocations()
    {
        if (!useTarget)
        {
            return U.FindNearestRoads(transform.position, 25f);
        }
        else
            return U.FindNearestRoads(target.transform.position, 25f);
    }

    void SpawnCar(GameObject node)
    {
        int carChoice = Random.Range(0, carList.Count);
        GameObject newCar = Instantiate(carList[carChoice], node.transform.position, Quaternion.identity);
        cars.Add(newCar);
    }

    IEnumerator CheckNumCars()
    {
        while (spawnCars)
        {
            int numRoads = RoadGenerator.instance.roads.Count;
            int difference = numRoads - (cars.Count * 3);
            if(difference >= 1 && ReferenceManager.instance.populationManager.totalPopulation >= 1)
            {
                SpawnCars(difference);
            }
            yield return new WaitForSeconds(5);
        }
    }
}
