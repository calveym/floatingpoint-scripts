using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    // Small size residential buildings
    Object[] res;
    Object[] com;
    Object[] ind;
    Object[] off;
    Object[] leis;
    Object[] util;
    Object[] indc;
    Object[] fol;

    List<Object[]> buildingList;

    public GameObject sphere0;
    public GameObject sphere1;
    public GameObject sphere2;
    public GameObject sphere3;
    public GameObject sphere4;

    SpawnController spawnController0;
    SpawnController spawnController1;
    SpawnController spawnController2;
    SpawnController spawnController3;
    SpawnController spawnController4;

    private void Awake()
    {
        LoadAllBuildings();
    }

    private void Start()
    {
        FindSpheres();
    }

    public GameObject Spawn(Vector3 targetPosition, int type, int unit)
    // Starts spawn process, enables static spawn calls
    {
        return Instantiate(FindBuilding(type, unit), targetPosition, Quaternion.identity) as GameObject;
    }

    public void SpawnUIBuildings(int type, int addNumber)
    {
        Debug.Log("Running at all");
        if(!sphere0 || !sphere1 || !sphere2 || !sphere3 || !sphere4)
        {
            FindSpheres();
        }
        if(spawnController0.Empty())
        {
            Debug.Log("Tryna spawn at 0");
            GameObject newBuilding = Spawn(sphere0.transform.position, type, 0 + addNumber);
            spawnController0.DisableBuilding(this, newBuilding);

        }
        if (spawnController1.Empty())
        {
            Debug.Log("Tryna spawn at 1");
            GameObject newBuilding = Spawn(sphere1.transform.position, type, 1 + addNumber);
            spawnController1.DisableBuilding(this, newBuilding);
        }
        if(spawnController2.Empty())
        {
            Debug.Log("Tryna spawn at 2");
            GameObject newBuilding = Spawn(sphere2.transform.position, type, 2 + addNumber);
            spawnController2.DisableBuilding(this, newBuilding);
        }
        if(spawnController3.Empty())
        {
            Debug.Log("Tryna spawn at 3");
            GameObject newBuilding = Spawn(sphere3.transform.position, type, 3 + addNumber);
            spawnController3.DisableBuilding(this, newBuilding);
        }
        if(spawnController4.Empty())
        {
            Debug.Log("Tryna spawn at 4");
            GameObject newBuilding = Spawn(sphere4.transform.position, type, 4 + addNumber);
            spawnController4.DisableBuilding(this, newBuilding);
        }
    }

    public int GetNumBuildings(int id)
    {
        return buildingList[id].Length;
    }

    void FindSpheres()
    {
        spawnController0 = sphere0.GetComponent<SpawnController>();
        spawnController1 = sphere1.GetComponent<SpawnController>();
        spawnController2 = sphere2.GetComponent<SpawnController>();
        spawnController3 = sphere3.GetComponent<SpawnController>();
        spawnController4 = sphere4.GetComponent<SpawnController>();
    }

    Object FindBuilding(int type, int unit)
    {
        return buildingList[type][unit];
    }

    void LoadAllBuildings()
    // Loads buildings from resources folder
    {
        res = Resources.LoadAll("res");
        com = Resources.LoadAll("com");
        ind = Resources.LoadAll("ind");
        off = Resources.LoadAll("off");
        leis = Resources.LoadAll("leis");
        util = Resources.LoadAll("util");
        indc = Resources.LoadAll("indc");
        fol = Resources.LoadAll("fol");

        buildingList = new List<Object[]>();
        buildingList.Add(res);
        buildingList.Add(com);
        buildingList.Add(ind);
        buildingList.Add(off);
        buildingList.Add(indc);
        buildingList.Add(fol);
    }

}
