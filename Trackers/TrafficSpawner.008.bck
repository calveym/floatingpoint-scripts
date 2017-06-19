using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour {

    PopulationManager populationManager;
    public static TrafficSpawner instance;  // singleton instance

    List<GameObject> cars;  // List of all cars in scene
    bool spawnCars;  // Controls car spawning

    [Tooltip("Alternate spawn check location")]
    public GameObject target;

    public GameObject carA;
    public GameObject carB;
    public GameObject carC;
    List<GameObject> carList;  // List of spawnable cars

    public GameObject testObject;

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

    void SpawnCars(int maxCars, List<GameObject> spawnLocations)
    {        
        foreach(GameObject location in spawnLocations)
        {
            //Debug.Log(location + "LOCATIONNNN");
            if (cars.Count >= maxCars)
                break;
            int side = Random.Range(0, 2);
            if(side == 0)
            {
                foreach(Transform child in location.transform)
                {
                    if (child.name == "StraightRightStart")
                    {
                        SpawnCar(child.gameObject);
                        break;
                    }
                }
            }
            else if(side == 1)
            {
                foreach (Transform child in location.transform)
                {
                    if (child.name == "StraightLeftStart")
                    {
                        SpawnCar(child.gameObject);
                        break;
                    }
                }
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
        List<GameObject> allRoads;
        if (testObject)
        {
            allRoads = U.FindNearestRoads(testObject.transform.position, 25f);
        }
        else
        {
            allRoads = U.FindNearestRoads(transform.position, 25f);
        }
        List<GameObject> roadsToSpawn = new List<GameObject>();
		foreach (GameObject road in allRoads) {
			if (U.FindNearestBuildings (road.transform.position, 1.1f).Count > 0) {
                roadsToSpawn.Add (road);
            }
		}
		return roadsToSpawn;
    }

    void SpawnCar(GameObject node)
    {
        int carChoice = Random.Range(0, carList.Count);
        GameObject newCar = Instantiate(carList[carChoice], node.transform.position, Quaternion.identity);
        cars.Add(newCar);
    }

	int carSpawnAndPopulationCheck(int carsToSpawn, int totalPopulation) {
		if (carsToSpawn > totalPopulation) {
			return totalPopulation;
		} else {
			return carsToSpawn;
		}
	}

    IEnumerator CheckNumCars()
    {
        while (spawnCars)
        {
			int numRoads = RoadGenerator.instance.roads.Count;
			int maxCars = Mathf.RoundToInt (numRoads / 6);
            if(maxCars > ReferenceManager.instance.populationManager.totalPopulation)
            {
                maxCars = ReferenceManager.instance.populationManager.totalPopulation;
            }
            if (maxCars >= 1) {
                List<GameObject> spawnLocations = GetSpawnLocations();
                if (spawnLocations.Count < maxCars)
                    maxCars = spawnLocations.Count;
                SpawnCars(maxCars, spawnLocations);
			}

            yield return new WaitForSeconds(5);
        }
    }
}
