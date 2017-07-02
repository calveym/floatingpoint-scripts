using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

public class DestroyPlane : MonoBehaviour {

    ItemManager itemManager;

    private void Start()
    {if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
    }

	void OnTriggerEnter(Collider other) {
        string tag = other.gameObject.tag;
        if (tag == "mayorsoffice")
        {
            other.gameObject.transform.position = new Vector3(0f, 10f, 0f);
        }
        else if (tag == "residential")
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
        else if(tag == "service")
        {
            //itemManager.removeService(other.gameObject);
        }
        else if(tag == "foliage")
        {
            itemManager.removeFoliage(other.gameObject);
        }
		Destroy(other.gameObject);
	}
}
