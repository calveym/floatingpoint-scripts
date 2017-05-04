using System.Collections;
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

    public GameObject smlCommercial1;

    public GameObject smlIndustrial1;

    public GameObject smlFoliage1;

    public GameObject smlLeisure1;

    public GameObject midResidential1;

    public GameObject midCommercial1;

    public GameObject midIndustrial1;

    public GameObject midFoliage1;

    public GameObject midLeisure1;

    public GameObject lrgResidential1;

    public GameObject lrgCommercial1;

    public GameObject lrgIndustrial1;

    public GameObject lrgFoliage1;

    public GameObject lrgLeisure1;

    private void Start()
    {
        smlRes = new GameObject[3] { smlResidential1, smlResidential1, smlResidential1 };
        smlCom = new GameObject[3] { smlCommercial1, smlCommercial1, smlCommercial1 };
        smlInd = new GameObject[3] { smlIndustrial1, smlIndustrial1, smlIndustrial1 };
        smlFol = new GameObject[3] { smlFoliage1, smlFoliage1, smlFoliage1 };
        smlLes = new GameObject[3] { smlLeisure1, smlLeisure1, smlLeisure1 };

        midRes = new GameObject[3] { smlResidential1, midResidential1, midResidential1 };
        midCom = new GameObject[3] { midCommercial1, midCommercial1, midCommercial1 };
        midInd = new GameObject[3] { midIndustrial1, midIndustrial1, midIndustrial1 };
        midFol = new GameObject[3] { midFoliage1, midFoliage1, midFoliage1 };
        midLes = new GameObject[3] { smlLeisure1, smlLeisure1, smlLeisure1 };

        lrgRes = new GameObject[3] { lrgResidential1, lrgResidential1, lrgResidential1 };
        lrgCom = new GameObject[3] { lrgCommercial1, lrgCommercial1, lrgCommercial1 };
        lrgInd = new GameObject[3] { lrgIndustrial1, lrgIndustrial1, lrgIndustrial1 };
        lrgFol = new GameObject[3] { lrgFoliage1, lrgFoliage1, lrgFoliage1 };
        lrgLes = new GameObject[3] { smlLeisure1, smlLeisure1, smlLeisure1 };

		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
    }

    public GameObject StartSpawn(int pressedButton, GameObject initiator)
    // Starts spawn cycle by selecting correct model
    {
        // Debug.Log(pressedButton);
        // Debug.Log(initiator);
        if (pressedButton == 1 && initiator.name == "Model1")
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
			newObject.GetComponent<ResidentialTracker>().setValues();
		}
		else if(newObject.tag == "commercial")
		{
			newObject.GetComponent<CommercialTracker>().setValues();
		}
		else if(newObject.tag == "industrial")
		{
			newObject.GetComponent<IndustrialTracker>().setValues();
		}
		else if(newObject.tag == "foliage")
		{
		}
	}
}
