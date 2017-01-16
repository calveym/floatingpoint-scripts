using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateItems : MonoBehaviour {

	public GameObject controller;
	ItemManager itemManager;

	public GameObject[] lrgCommercial;
	public GameObject[] lrgIndustrial;
	public GameObject[] lrgResidential;
	public GameObject[] smlCommercial;
	public GameObject[] smlIndustrial;
	public GameObject[] smlResidential;
	public GameObject[] trees;
	public GameObject[] leisure;
	public GameObject[] bushes;
	public GameObject[] vehicles;
	public GameObject[] propsArray1;
	public GameObject[] propsArray2;

	public GameObject lrgCommercial1;
	public GameObject lrgCommercial2;
	public GameObject lrgCommercial3;
	public GameObject lrgCommercial4;
	public GameObject lrgCommercial5;
	public GameObject lrgCommercial6;

	public GameObject lrgResidential1;
	public GameObject lrgResidential2;

	public GameObject lrgIndustrial1;
	public GameObject lrgIndustrial2;
	public GameObject lrgIndustrial3;
	public GameObject lrgIndustrial4;
	public GameObject lrgIndustrial5;
	public GameObject lrgIndustrial6;

	public GameObject smlCommercial1;
	public GameObject smlCommercial2;
	public GameObject smlCommercial3;
	public GameObject smlCommercial4;
	public GameObject smlCommercial5;
	public GameObject smlCommercial6;

	public GameObject smlResidential1;
	public GameObject smlResidential2;
	public GameObject smlResidential3;
	public GameObject smlResidential4;
	public GameObject smlResidential5;
	public GameObject smlResidential6;

	public GameObject smlIndustrial1;
	public GameObject smlIndustrial2;
	public GameObject smlIndustrial3;
	public GameObject smlIndustrial4;
	public GameObject smlIndustrial5;
	public GameObject smlIndustrial6;

	public GameObject tree1;
	public GameObject tree2;
	public GameObject tree3;
	public GameObject tree4;
	public GameObject tree5;
	public GameObject tree6;

	public GameObject bush1;
	public GameObject bush2;
	public GameObject bush3;
	public GameObject bush4;
	public GameObject bush5;
	public GameObject bush6;

	public GameObject leisure1;
	public GameObject leisure2;
	public GameObject leisure3;
	public GameObject leisure4;
	public GameObject leisure5;

	public GameObject vehicle1;
	public GameObject vehicle2;
	public GameObject vehicle3;
	public GameObject vehicle4;
	public GameObject vehicle5;
	public GameObject vehicle6;

	public GameObject props1;
	public GameObject props2;
	public GameObject props3;
	public GameObject props4;
	public GameObject props5;
	public GameObject props6;

	public GameObject props7;
	public GameObject props8;
	public GameObject props9;
	public GameObject props10;
	public GameObject props11;
	public GameObject props12;

	void Start()
		{
		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();

		lrgCommercial = new GameObject[6] {lrgCommercial1, lrgCommercial2, lrgCommercial3, lrgCommercial4, lrgCommercial5, lrgCommercial6 };
		lrgResidential = new GameObject[2] { lrgResidential1, lrgResidential2 };
		lrgIndustrial = new GameObject[6] {lrgIndustrial1, lrgIndustrial2, lrgIndustrial3, lrgIndustrial4, lrgIndustrial5, lrgIndustrial6 };
		smlCommercial = new GameObject[6] { smlResidential1, smlResidential2, smlResidential3, smlResidential4, smlResidential5, smlResidential6};
		smlResidential = new GameObject[6] { smlCommercial1, smlCommercial2, smlCommercial3, smlCommercial4, smlCommercial5, smlCommercial6};
		smlIndustrial = new GameObject[6] { smlIndustrial1, smlIndustrial2, smlIndustrial3, smlIndustrial4, smlIndustrial5, smlIndustrial6};
		trees = new GameObject[6] { tree1, tree2, tree3, tree4, tree5, tree6 };
		leisure = new GameObject[5] { leisure1, leisure2, leisure3, leisure4, leisure5 };
		bushes = new GameObject[6] { bush1, bush2, bush3, bush4, bush5, bush6};
		vehicles = new GameObject[6] { vehicle1, vehicle2, vehicle3, vehicle4, vehicle5, vehicle6};
		propsArray1 = new GameObject[6] { props1, props2, props3, props4, props5, props6};
		propsArray2 = new GameObject[6] { props7, props8, props9, props10, props11, props12};
		}

	void RandomGenerator(GameObject[] array, out int index) {
		index = Random.Range (0, array.Length);
	}

    public void GenerateButton1()
    {
    	int index;
    	itemManager.addCommercial(5);
		RandomGenerator(lrgCommercial, out index);
		Object.Instantiate(lrgCommercial[index], controller.transform.position , Quaternion.identity);
    }

	public void GenerateButton2()
	{
    	int index;
    	itemManager.addResidential(5);
		RandomGenerator(lrgResidential, out index);
		Object.Instantiate(lrgResidential[index], controller.transform.position , Quaternion.identity);
    }

	public void GenerateButton3()
	{
    	int index;
    	itemManager.addIndustrial(5);
		RandomGenerator(lrgIndustrial, out index);
		Object.Instantiate(lrgIndustrial[index], controller.transform.position , Quaternion.identity);
    }


	public void GenerateButton4()
	{
    	int index;
    	itemManager.addCommercial(1);
		RandomGenerator(smlCommercial, out index);
		Object.Instantiate(smlCommercial[index], controller.transform.position , Quaternion.identity);
    }


	public void GenerateButton5()
	{
    	int index;
		itemManager.addIndustrial(1);
		RandomGenerator(smlIndustrial, out index);
		Object.Instantiate(smlIndustrial[index], controller.transform.position , Quaternion.identity);
    }

	public void GenerateButton6()
	{
    	int index;
		itemManager.addResidential(1);
		RandomGenerator(smlResidential, out index);
		Object.Instantiate(smlResidential[index], controller.transform.position , Quaternion.identity);
    }


	public void GenerateButton7()
	{
    	int index;
		RandomGenerator(trees, out index);
		Object.Instantiate(trees[index], controller.transform.position , Quaternion.identity);
    }


	public void GenerateButton8()
	{
    	int index;
		RandomGenerator(leisure, out index);
		Object.Instantiate(leisure[index], controller.transform.position , Quaternion.identity);
    }


	public void GenerateButton9()
	{
    	int index;
		RandomGenerator(bushes, out index);
		Object.Instantiate(bushes[index], controller.transform.position , Quaternion.identity);
    }

	public void GenerateButton10()
	{
    	int index;
		RandomGenerator(vehicles, out index);
		Object.Instantiate(vehicles[index], controller.transform.position , Quaternion.identity);
    }

	public void GenerateButton11()
	{
    	int index;
		RandomGenerator(propsArray1, out index);
		Object.Instantiate(propsArray1[index], controller.transform.position , Quaternion.identity);
    }


	public void GenerateButton12()
	{
    	int index;
		RandomGenerator(propsArray2, out index);
		Object.Instantiate(propsArray2[index], controller.transform.position , Quaternion.identity);
    }

}
