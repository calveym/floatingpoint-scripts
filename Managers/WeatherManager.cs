using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

    [Header("Controls")]
    [Tooltip("Current weather state \n [0:sun, 1:cloudy, 2:rainy]")]
    public int weatherState = 0;  // 0 = Sun, 1 = cloudy, 2 = rainy
    int oldWeatherState = 0;
    [Tooltip("Enable random weather updates")]
    public bool randomiseWeather = true;
    System.Random r = new System.Random();
    TOD_Sky tod;

    [Header("-------Rain Objects-------")]
    public GameObject rainSystem;  // Contains rain particles and clouds
	public GameObject drizzleSystem; // Contains drizzle particles and clouds

	public ParticleSystem[] rainParticleSystem;
	public ParticleSystem[] drizzleParticleSystem;

    AudioManager audioManager;

	// Use this for initialization
	void Start () {
        audioManager = ReferenceManager.instance.audioManager;
        tod = ReferenceManager.instance.tod;
        ToggleRain(false);
		ToggleDrizzle (false);
		rainSystem.SetActive (true);
		drizzleSystem.SetActive (true);
        ToggleClouds(0.1f);
		rainParticleSystem = rainSystem.GetComponentsInChildren<ParticleSystem> ();
		drizzleParticleSystem = drizzleSystem.GetComponentsInChildren<ParticleSystem> ();
        StartCoroutine("MakeItRain");

		TestRunner.first += Rainy;
        TestRunner.second += Cloudy;
        TestRunner.third += Drizzle;
        TestRunner.fourth += Sunny;
	}
	
    void ReloadWeather()
    {
        switch (weatherState)
        {
            case 0:
                // Sunny
                Sunny();
                break;
            case 1:
                // Cloudy
                Cloudy();
                break;
            case 2:
                // Drizzle
                Drizzle();
                break;
			case 3:
				// Rainy
				Rainy();
				break;
        }
    }

    void Sunny()
    {
        audioManager.StopRain();
        ToggleClouds(0.1f);
        tod.Day.LightIntensity = 1f;
        tod.Day.AmbientMultiplier = 1f;
        //tod.Night.LightIntensity = 1.5f;
        //tod.Night.AmbientMultiplier = 4.26f;
        ToggleRain(false);
		ToggleDrizzle(false);
    }

    void Cloudy()
    {
        audioManager.StopRain();
        ToggleClouds(0.5f);
        tod.Day.LightIntensity = 0.75f;
        tod.Day.AmbientMultiplier = 0.5f;
        //tod.Night.LightIntensity = 1.16f;
        //tod.Night.AmbientMultiplier = 2.72f;
        ToggleRain(false);
		ToggleDrizzle(false);
    }

	void Drizzle()
	{
		audioManager.PlayRain();
		ToggleClouds(0.6f);
		tod.Day.LightIntensity = 0.7f;
		tod.Day.AmbientMultiplier = 0.5f;
		//tod.Night.LightIntensity = 1.16f;
		//tod.Night.AmbientMultiplier = 2.72f;
		ToggleRain(false);
		ToggleDrizzle(true);
	}

    void Rainy()
    {
        audioManager.PlayRain();
        ToggleClouds(1f);
        tod.Day.LightIntensity = 0.38f;
        tod.Day.AmbientMultiplier = 0.6f;
        //tod.Night.LightIntensity = 0.88f;
        //tod.Night.AmbientMultiplier = 4.54f;
        ToggleRain(true);
		ToggleDrizzle(false);
    }

    void ToggleClouds(float intensity)
    // Takes float between 0-1 for coverage
    {
        tod.Clouds.Coverage = intensity;
    }

    void ToggleRain(bool toggle)
    {
		if (toggle == false) {
			foreach(ParticleSystem rain in rainParticleSystem) {
				rain.Stop ();
                ParticleSystem.EmissionModule em = rain.emission;
                em.enabled = false;
            }
		} else if (toggle == true) {
			foreach(ParticleSystem rain in rainParticleSystem) {
				rain.Play ();
                ParticleSystem.EmissionModule em = rain.emission;
                em.enabled = true;
            }
		}
    }

	void ToggleDrizzle(bool toggle)
	{
		if (toggle == false) {
			foreach(ParticleSystem drizzle in drizzleParticleSystem) {
                drizzleSystem.SetActive(false);
                drizzle.Stop ();
                ParticleSystem.EmissionModule em = drizzle.emission;
                em.enabled = false;
            }
		} else if (toggle == true) {
			foreach(ParticleSystem drizzle in drizzleParticleSystem) {
                drizzleSystem.SetActive(true);
                drizzle.Play ();
                ParticleSystem.EmissionModule em = drizzle.emission;
                em.enabled = true;
            }
		}
	}

    IEnumerator MakeItRain()
    {
        while(randomiseWeather)
        {
			int number = Random.Range(0, 11);

			if (number == 1) {
				if (weatherState == 3) {
					yield return new WaitForSeconds (60);
				} else {
					weatherState = 3;
				}
			} else if (number == 2) {
				if (weatherState == 2) {
					yield return new WaitForSeconds (60);
				} else {
					weatherState = 2;
				}
			} else if (number == 3 || number == 4) {
				weatherState = 1;
			} else {
				weatherState = 0;
			}

			ReloadWeather ();

            yield return new WaitForSeconds(60);
        }
    }
}
