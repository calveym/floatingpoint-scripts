﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour {

    Random rnd;
	ItemManager itemManager;

    // Small model arrays
    GameObject[] smlRes;
    GameObject[] smlCom;
    GameObject[] smlInd;
    GameObject[] smlFol;
    GameObject[] smlLes;

    // Medium model arrays
    GameObject[] midRes;
    GameObject[] midCom;
    GameObject[] midInd;
    GameObject[] midFol;
    GameObject[] midLes;

    // Large model arrays
    GameObject[] lrgRes;
    GameObject[] lrgCom;
    GameObject[] lrgInd;
    GameObject[] lrgFol;
    GameObject[] lrgLes;

    // Models
    public GameObject smlResidential1;
    public GameObject smlResidential2;
    public GameObject smlResidential3;

    public GameObject smlCommercial1;
    public GameObject smlCommercial2;
    public GameObject smlCommercial3;

    public GameObject smlIndustrial1;
    public GameObject smlIndustrial2;
    public GameObject smlIndustrial3;

    public GameObject smlFoliage1;
    public GameObject smlFoliage2;
    public GameObject smlFoliage3;

    public GameObject smlLeisure1;
    public GameObject smlLeisure2;
    public GameObject smlLeisure3;

    public GameObject midResidential1;
    public GameObject midResidential2;
    public GameObject midResidential3;

    public GameObject midCommercial1;
    public GameObject midCommercial2;
    public GameObject midCommercial3;

    public GameObject midIndustrial1;
    public GameObject midIndustrial2;
    public GameObject midIndustrial3;

    public GameObject midFoliage1;
    public GameObject midFoliage2;
    public GameObject midFoliage3;

    public GameObject midLeisure1;
    public GameObject midLeisure2;
    public GameObject midLeisure3;

    public GameObject lrgResidential1;
    public GameObject lrgResidential2;
    public GameObject lrgResidential3;

    public GameObject lrgCommercial1;
    public GameObject lrgCommercial2;
    public GameObject lrgCommercial3;

    public GameObject lrgIndustrial1;
    public GameObject lrgIndustrial2;
    public GameObject lrgIndustrial3;

    public GameObject lrgFoliage1;
    public GameObject lrgFoliage2;
    public GameObject lrgFoliage3;

    public GameObject lrgLeisure1;
    public GameObject lrgLeisure2;
    public GameObject lrgLeisure3;

    private void Start()
    {
        smlRes = new GameObject[3] { smlResidential1, smlResidential2, smlResidential3 };
        smlCom = new GameObject[3] { smlCommercial1, smlCommercial2, smlCommercial3 };
        smlInd = new GameObject[3] { smlIndustrial1, smlIndustrial2, smlIndustrial3 };
        smlFol = new GameObject[3] { smlFoliage1, smlFoliage2, smlFoliage3 };
        smlLes = new GameObject[3] { smlFoliage1, smlFoliage2, smlFoliage3 };

        midRes = new GameObject[3] { midResidential1, midResidential2, midResidential3 };
        midCom = new GameObject[3] { midCommercial1, midCommercial2, midCommercial3 };
        midInd = new GameObject[3] { midIndustrial1, midIndustrial2, midIndustrial3 };
        midFol = new GameObject[3] { midFoliage1, midFoliage2, midFoliage3 };
        midLes = new GameObject[3] { midFoliage1, midFoliage2, midFoliage3 };

        lrgRes = new GameObject[3] { lrgResidential1, lrgResidential2, lrgResidential3 };
        lrgCom = new GameObject[3] { lrgCommercial1, lrgCommercial2, lrgCommercial3 };
        lrgInd = new GameObject[3] { lrgIndustrial1, lrgIndustrial2, lrgIndustrial3 };
        lrgFol = new GameObject[3] { lrgFoliage1, lrgFoliage2, lrgFoliage3 };
        lrgLes = new GameObject[3] { lrgFoliage1, lrgFoliage2, lrgFoliage3 };

		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
    }

    public GameObject StartSpawn(int pressedButton, GameObject initiator)
    // Starts spawn cycle by selecting correct model
    {
		// Debug.Log(pressedButton);
		// Debug.Log(initiator);
        if(pressedButton == 1 && initiator.name == "Model1")
        {
            return Spawn(smlRes, initiator, pressedButton);
        }
        else if (pressedButton == 1 && initiator.name == "Model2")
        {
            return Spawn(smlCom, initiator, pressedButton);
        }
        else if(pressedButton == 1 && initiator.name == "Model3")
        {
            return Spawn(smlInd, initiator, pressedButton);
        }
        else if(pressedButton == 1 && initiator.name == "Model4")
        {
            return Spawn(smlFol, initiator, pressedButton);
        }
        else if(pressedButton == 1 && initiator.name == "Model5")
        {
            return Spawn(smlLes, initiator, pressedButton);
        }
        else if (pressedButton == 2 && initiator.name == "Model1")
        {
            return Spawn(midRes, initiator, pressedButton);
        }
        else if (pressedButton == 2 && initiator.name == "Model2")
        {
            return Spawn(midCom, initiator, pressedButton);
        }
        else if (pressedButton == 2 && initiator.name == "Model3")
        {
            return Spawn(midInd, initiator, pressedButton);
        }
        else if (pressedButton == 2 && initiator.name == "Model4")
        {
            return Spawn(midFol, initiator, pressedButton);
        }
        else if (pressedButton == 2 && initiator.name == "Model5")
        {
            return Spawn(midLes, initiator, pressedButton);
        }
        else if (pressedButton == 3 && initiator.name == "Model1")
        {
            return Spawn(lrgRes, initiator, pressedButton);
        }
        else if (pressedButton == 3 && initiator.name == "Model2")
        {
            return Spawn(lrgCom, initiator, pressedButton);
        }
        else if (pressedButton == 3 && initiator.name == "Model3")
        {
            return Spawn(lrgInd, initiator, pressedButton);
        }
        else if (pressedButton == 3 && initiator.name == "Model4")
        {
            return Spawn(lrgFol, initiator, pressedButton);
        }
        else if (pressedButton == 3 && initiator.name == "Model5")
        {
            return Spawn(lrgLes, initiator, pressedButton);
        }
		else
		{
			return new GameObject();
		}
    }

    GameObject Spawn(GameObject[] modelArray, GameObject initiator, int pressedButton)
    // Instantiates new object from array
    {
        RemoveChildren(initiator);
        int r = (int)Mathf.Round(Random.Range(0f, modelArray.Length - 1));
        Vector3 spawnPosition = initiator.transform.position;
        GameObject newObject = Instantiate(modelArray[r], spawnPosition, Quaternion.identity);
		AddObjectInfo(newObject, pressedButton);
		newObject.transform.parent = initiator.transform;
		newObject.layer = 5;
		newObject.GetComponent<Rigidbody>().isKinematic = true;
		return newObject;
    }
    void RemoveChildren(GameObject initiator)
    {
        if(initiator.transform.childCount == 1)
        {
            initiator.transform.GetChild(0).transform.parent = null;
        }
    }

	void AddObjectInfo(GameObject newObject, int pressedButton)
	// Adds type info to itemTracker and itemManager
	{
		if(newObject.tag == "residential")
		{
			newObject.GetComponent<ResidentialTracker>().setValues("residential", pressedButton);
		}
		else if(newObject.tag == "commercial")
		{
			newObject.GetComponent<CommercialTracker>().setValues("commercial", pressedButton);
		}
		else if(newObject.tag == "industrial")
		{
			newObject.GetComponent<IndustrialTracker>().setValues("industrial", pressedButton);
		}
		else if(newObject.tag == "foliage")
		{
			newObject.GetComponent<ItemTracker>().setValues("foliage", pressedButton);
		}
		else if(newObject.tag == "leisure")
		{
            newObject.GetComponent<ItemTracker>().setValues("leisure", pressedButton);
		}
	}
}
