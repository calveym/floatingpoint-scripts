using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;


public class NightTime : MonoBehaviour {

    public static NightTime instance;
	TOD_Sky tod;
	private bool lightsOn = true;
	public Material mat;
	private float intensity;

	void Start() 
	{
        if (instance == null)
            instance = this;

		tod = ReferenceManager.instance.tod;
		intensity = 0.75f;

        Autelia.Serialization.Serializer.OnSerializationStart -= StopAll;
        Autelia.Serialization.Serializer.OnSerializationStart += StopAll;

        Autelia.Coroutines.CoroutineController.StartCoroutine(this, "LightUpBuildings");
	}

    void StopAll()
    {
        Debug.Log("Stopping routines");
        Autelia.Coroutines.CoroutineController.StopCoroutine(this, "LightUpBuildings");
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
