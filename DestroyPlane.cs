using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlane : MonoBehaviour {

    ItemManager itemManager;

    private void Awake()
    {
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
    }

	void OnTriggerEnter(Collider other) {
        string tag = other.gameObject.tag;
        if (tag == "residential")
        {
            itemManager.removeResidential(other.gameObject);
        }
        else if(tag == "commercial")
        {
            itemManager.removeCommercial(other.gameObject);
        }
        else if(tag == "industrial")
        {
            itemManager.removeIndustrial(other.gameObject);
        }
        else if(tag == "leisure")
        {
            itemManager.removeLeisure(other.gameObject);
        }
        else if(tag == "foliage")
        {
            itemManager.removeFoliage(other.gameObject);
        }
		Destroy(other.gameObject);
	}
}
