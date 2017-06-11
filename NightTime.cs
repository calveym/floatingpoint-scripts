using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightTime : MonoBehaviour {

	public Material Streetlight;
	public Material Buildinglight;
	public GameObject[] Lamps;
	TOD_Sky tod;

	void Start() 
	{
		Lamps = GameObject.FindGameObjectsWithTag ("StreetLight");
		tod = ReferenceManager.instance.tod;
	}

	void Update()
	{
		if (tod.IsNight == true) 
		{
			Buildinglight.SetColor ("_EmissionColor", Color.yellow);
			Streetlight.SetColor ("_EmissionColor", Color.yellow);
			for(int i = 0; i < Lamps.Length; i++)
			{
				Lamps [i].GetComponent<Light>().enabled = true;
			}

		} 
		else 
		{
			Buildinglight.SetColor ("_EmissionColor", Color.black);
			Streetlight.SetColor ("_EmissionColor", Color.black);
			for(int i = 0; i < Lamps.Length; i++)
			{
				Lamps [i].GetComponent<Light>().enabled = false;
			}
		}
	}
}
