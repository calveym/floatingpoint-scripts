using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

    [Header("Controls")]
    [Tooltip("Current weather state \n [0:sun, 1:cloudy, 2:rainy]")]
    public int weatherState = 0;  // 0 = Sun, 1 = cloudy, 2 = rainy
    int oldWeatherState = 0;
    [Tooltip("Enable random weather updates")]
    public bool randomiseWeather = false;
    System.Random r = new System.Random();
    TOD_Sky tod;

    [Header("-------Rain Objects-------")]
    public GameObject rainSystem;  // Contains rain particles and clouds
    public GameObject rainScreen;  // Contains rain screen effects

    AudioManager audioManager;

	// Use this for initialization
	void Start () {
        audioManager = ReferenceManager.instance.audioManager;
        tod = ReferenceManager.instance.tod;
        ToggleRain(false);
        ToggleClouds(0.1f);
        //StartCoroutine("MakeItRain");

        TestRunner.first += Sunny;
        TestRunner.second += Cloudy;
        TestRunner.third += Rainy;
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
        tod.Day.AmbientMultiplier = 0.5f;
        //tod.Night.LightIntensity = 1.5f;
        //tod.Night.AmbientMultiplier = 4.26f;
        ToggleRain(false);
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
    }

    void ToggleClouds(float intensity)
    // Takes float between 0-1 for coverage
    {
        tod.Clouds.Coverage = intensity;
    }

    void ToggleRain(bool toggle)
    {
        rainSystem.SetActive(toggle);
        rainScreen.SetActive(toggle);
    }

    IEnumerator MakeItRain()
    {
        while(randomiseWeather)
        {
            if(weatherState == 0)
            {
                if(r.Next(100) <= 20)
                {
                    weatherState = 1;
                }
            }
            else if(weatherState == 1)
            {
                if(r.Next(100) <= 65)
                {
                    weatherState = 0;
                }
            }
            if (weatherState != oldWeatherState)
                ReloadWeather();
            yield return new WaitForSeconds(15);
        }
    }
}
