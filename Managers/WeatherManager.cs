using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour {

    int weatherState = 0;  // 0 = Sun, 1 = cloudy, 2 = rainy
    int oldWeatherState = 0;
    bool randomiseWeather = true;
    System.Random r = new System.Random();
    TOD_Sky tod;

    [Header("-------Rain Objects-------")]
    public GameObject rainSystem;  // Contains rain particles and clouds
    public GameObject rainScreen;  // Contains rain screen effects

	// Use this for initialization
	void Start () {
        tod = ReferenceManager.instance.tod;
        ToggleRain(false);
        ToggleClouds(0.1f);
        StartCoroutine("MakeItRain");

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
        ToggleClouds(0.1f);
        tod.Day.LightIntensity = 0.8f;
        tod.Day.AmbientMultiplier = 0.5f;
        //tod.Night.LightIntensity = 1.5f;
        //tod.Night.AmbientMultiplier = 4.26f;
        ToggleRain(false);
    }

    void Cloudy()
    {
        ToggleClouds(0.5f);
        tod.Day.LightIntensity = 0.7f;
        tod.Day.AmbientMultiplier = 0.5f;
        //tod.Night.LightIntensity = 1.16f;
        //tod.Night.AmbientMultiplier = 2.72f;
        ToggleRain(false);
    }

    void Rainy()
    {
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
