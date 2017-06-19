using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightTime : MonoBehaviour {

	TOD_Sky tod;
	private bool lightsOn = true;
	public Material mat;
	Color colour;
	private float intensity;

	void Start() 
	{
		tod = ReferenceManager.instance.tod;
		intensity = 0.75f;
		StartCoroutine("LightUpBuildings");
	}

	IEnumerator LightUpBuildings()
	{
		while(lightsOn)
		{
			if (tod.Cycle.Hour >= 18.00 || tod.Cycle.Hour <= 6.00) 
			{
				mat.SetColor ("_EmissionColor", Color.white * intensity);
			} 
			else 
			{
				mat.SetColor ("_EmissionColor", Color.black);
			}
				
			yield return new WaitForSeconds(5);
		}
	}
}
