using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC.TimeOfDaySystemFree;

public class NightTime : MonoBehaviour {

	public Material Streetlight;
	public Material Buildinglight;
	public float time;
	private GameObject[] Lamps;

	void Start() 
	{
		Lamps = GameObject.FindGameObjectsWithTag ("StreetLight");
	}

	void Update()
	{
		time = GameObject.FindGameObjectWithTag ("TimeManager").GetComponent<TimeOfDayManager> ().timeline;
		if (time <= 5.49f || time>= 18.00f) 
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
