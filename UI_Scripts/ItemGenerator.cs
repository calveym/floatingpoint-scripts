using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour {

    Random rnd;

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

    public GameObject smlFoliage1;
    public GameObject smlFoliage2;
    public GameObject smlFoliage3;

    public GameObject smlLeisure1;
    public GameObject smlLeisure2;
    public GameObject smlLeisure3;

    private void Start()
    {
        rnd = new Random();
    }

    public void StartSpawn(int pressedButton, GameObject initiator)
    // Starts spawn cycle by selecting correct model
    {
        if(pressedButton == 1 && initiator.name == "Model1")
        {
            Spawn(smlRes, initiator);
        }
        else if (pressedButton == 1 && initiator.name == "Model2")
        {
            Spawn(smlCom, initiator);
        }
        else if(pressedButton == 1 && initiator.name == "Model3")
        {
            Spawn(smlInd, initiator);
        }
        else if(pressedButton == 1 && initiator.name == "Model4")
        {
            Spawn(smlFol, initiator);
        }
        else if(pressedButton == 1 && initiator.name == "Model5")
        {
            // TODO
        }
        else if (pressedButton == 2 && initiator.name == "Model1")
        {
            Spawn(midRes, initiator);
        }
        else if (pressedButton == 2 && initiator.name == "Model2")
        {
            Spawn(midCom, initiator);
        }
        else if (pressedButton == 2 && initiator.name == "Model3")
        {
            Spawn(midInd, initiator);
        }
        else if (pressedButton == 2 && initiator.name == "Model4")
        {
            Spawn(midFol, initiator);
        }
        else if (pressedButton == 2 && initiator.name == "Model5")
        {
            // TODO
        }
        else if (pressedButton == 3 && initiator.name == "Model1")
        {
            Spawn(lrgRes, initiator);
        }
        else if (pressedButton == 3 && initiator.name == "Model2")
        {
            Spawn(lrgCom, initiator);
        }
        else if (pressedButton == 3 && initiator.name == "Model3")
        {
            Spawn(lrgInd, initiator);
        }
        else if (pressedButton == 3 && initiator.name == "Model4")
        {
            Spawn(lrgFol, initiator);
        }
        else if (pressedButton == 3 && initiator.name == "Model5")
        {
            // TODO
        }
    }

    void Spawn(GameObject[] modelArray, GameObject initiator)
    // Instantiates new object from array
    {
        int r = (int)Mathf.Round(Random.Range(0f, modelArray.Length));
        Vector3 spawnPosition = initiator.transform.position;
        GameObject newObject = Instantiate(modelArray[r], spawnPosition, Quaternion.identity);
    }
}
