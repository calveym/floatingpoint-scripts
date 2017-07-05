using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class industrialSmokeNightDisable : MonoBehaviour {

	TOD_Sky tod;
	private bool manageSmoke = true;
	private ParticleSystem ps;

	void Start() 
	{
		tod = ReferenceManager.instance.tod;

        Autelia.Coroutines.CoroutineController.StartCoroutine(this, "turnOffSmokeAtNight");
	}

	IEnumerator turnOffSmokeAtNight()
	{
		while(manageSmoke)
		{
			if (tod.Cycle.Hour >= 18.00 || tod.Cycle.Hour <= 6.00) 
			{
				turnOffSmokeObject ();
			} 
			else 
			{
				turnOnSmokeObject ();
			}

			yield return new WaitForSeconds(5);
		}
	}

	void turnOffSmokeObject() {
		foreach (Transform child in transform) {
            if (child.tag != "overlay")
            {
                ps = child.gameObject.GetComponent<ParticleSystem> ();
			    ps.Stop();
                //ps.enableEmission = false;
            }
        }
	}

	void turnOnSmokeObject() {
		foreach (Transform child in transform) {
            if(child.tag != "overlay")
            {
                ps = child.gameObject.GetComponent<ParticleSystem>();
                ps.Play();
                //ps.enableEmission = true;
            }
		}
	}
}
