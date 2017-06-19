using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRTK;

public class SpawnManager : MonoBehaviour {


    public DisplayUI displayUI;
    // Small size residential buildings
    Object[] res;
    Object[] com;
    Object[] ind;
    Object[] off;
    Object[] leis;
    Object[] util;
    Object[] indc;
    Object[] fol;
    Object[] serv;

    List<Object[]> buildingList;

    public AudioClip notificationSound;
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

    VRTK_ControllerEvents events;

    private void Awake()
    {
        LoadAllBuildings();
    }

    private void Start()
    {
        FindSpheres();
        events = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();
    }

    public GameObject Spawn(Vector3 targetPosition, int type, int unit)
    // Starts spawn process, enables static spawn calls
    {
        return Instantiate(FindBuilding(type, unit), targetPosition, Quaternion.identity) as GameObject;
    }

    public void PurchaseBuilding()
    {
        if(!displayUI)
        {
            displayUI = GameObject.Find("UI").GetComponent<DisplayUI>();
        }
        if (displayUI.showBuildings)
        {
            spawnController2.EnableBuilding();
        }
    }

    public void ResetUIBuildings()
    {
        spawnController0.gameObject.SetActive(true);
        spawnController1.gameObject.SetActive(true);
        spawnController2.gameObject.SetActive(true);
        spawnController3.gameObject.SetActive(true);
        spawnController4.gameObject.SetActive(true);

        spawnController0.DeleteBuilding();
        spawnController1.DeleteBuilding();
        spawnController2.DeleteBuilding();
        spawnController3.DeleteBuilding();
        spawnController4.DeleteBuilding();
    }

    public void SpawnUIBuildings(int type, int addNumber)
    {
        GameObject newBuilding;
        ResetUIBuildings();
        if(!sphere0 || !sphere1 || !sphere2 || !sphere3 || !sphere4)
        {
            FindSpheres();
        }
        if(spawnController0.Empty())
        {
            //Debug.Log("Tryna spawn at 0");
            newBuilding = Spawn(spawnController0.GetInstantiatePosition(), type, 0 + addNumber);
            spawnController0.DisableBuilding(this, newBuilding);
        }
        if(spawnController1.Empty())
        {
            //Debug.Log("Tryna spawn at 1");
            newBuilding = Spawn(spawnController1.GetInstantiatePosition(), type, 1 + addNumber);
            spawnController1.DisableBuilding(this, newBuilding);
        }
        if(spawnController2.Empty())
        {
            //Debug.Log("Tryna spawn at 2");
            newBuilding = Spawn(spawnController2.GetInstantiatePosition(), type, 2 + addNumber);
            spawnController2.DisableBuilding(this, newBuilding);
        }
        if(spawnController3.Empty())
        {
            //Debug.Log("Tryna spawn at 3");
            newBuilding = Spawn(spawnController3.GetInstantiatePosition(), type, 3 + addNumber);
            spawnController3.DisableBuilding(this, newBuilding);
        }
        if(spawnController4.Empty())
        {
            //Debug.Log("Tryna spawn at 4");
            newBuilding = Spawn(spawnController4.GetInstantiatePosition(), type, 4 + addNumber);
            spawnController4.DisableBuilding(this, newBuilding);
        }
        else newBuilding = new GameObject();
        AudioSource.PlayClipAtPoint(notificationSound, newBuilding.gameObject.transform.position);
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
        serv = Resources.LoadAll("serv");

        buildingList = new List<Object[]>();
        buildingList.Add(res);
        buildingList.Add(com);
        buildingList.Add(ind);
        buildingList.Add(off);
        buildingList.Add(indc);
        buildingList.Add(fol);
        buildingList.Add(serv);
    }
}
