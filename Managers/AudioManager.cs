using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    [Header("State")]
    [Tooltip("Set automatically. Used to decide which ambience state to play.")]
    public int state;
    [Range(0, 23.999f)]
    [Tooltip("Current time of day")]
    public float time;
    [Tooltip("True if city reaches critical mass of population/ distance")]
    public bool city;  // Used to know if to play urban version
    bool oldCity;  // Used in comparison check
    [Tooltip("Will update next sound tick")]
    public bool updateRequired;

    [Header("Settings")]
    [Range(0, 2)]
    public float lowPitchRange = 0.95f;
    [Range(0, 2)]
    public float highPitchRange = 1.05f;
    [Range(0, 10)]
    [Tooltip("How long it takes for ambiences to transition.")]
    public float ambienceTransitionTime;
    [Range(0, 10)]
    [Tooltip("How long it takes for rain to transition in/ out")]
    public float rainTransitionTime;

    [Header("Audio Snapshots")]
    public AudioMixerSnapshot dayForest;
    public AudioMixerSnapshot dayCity;
    public AudioMixerSnapshot sunsetForest;
    public AudioMixerSnapshot sunsetCity;
    public AudioMixerSnapshot nightForest;
    public AudioMixerSnapshot nightCity;

    public AudioMixerSnapshot rain;
    public AudioMixerSnapshot noRain;
    public AudioSource efxSource;
    
    public static AudioManager instance = null;

    bool checkTime;

    // State 0 = Night
    // State 1 = Twilight
    // State 2 = Day

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        checkTime = true;
        city = false;
        time = 0;
        state = 0;
        updateRequired = true;
    }

    void Start()
    {
        StartCoroutine("TimeCheck");
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.loop = false;
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlayLoop(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.loop = true;
        efxSource.Play();
    }

    public void TryStopEfx(AudioClip clip)
    {
        if(clip == efxSource.clip)
        {
            efxSource.Stop();
        }
    }

    public void EfxVolume(float volume)
    {
        efxSource.volume = volume;
    }

    public void RandomizeSfx (params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    public void PlayRain()
    {
        rain.TransitionTo(rainTransitionTime);
    }

    public void StopRain()
    {
        noRain.TransitionTo(rainTransitionTime);
    }

    void UpdateTime()
    {
        time = ReferenceManager.instance.tod.Cycle.Hour;
    }

    int CalculateState(float newTime)
    {
        if (newTime > 0 && newTime < 5)
            return 0;
        else if (newTime >= 5 && newTime < 7.3f)
            return 1;
        else if (newTime >= 6.50f && newTime < 17.5f)
            return 2;
        else if (newTime >= 17.5f && newTime < 19)
            return 1;
        else if (newTime >= 19 && newTime < 24)
            return 0;
        else
            //Debug.Log("Not passing...");
            return 2;
    }

    void PlayNight()
    // choose which night ambience to play
    {
        if (city)
        {
            //Debug.Log("NightCity");
            nightCity.TransitionTo(ambienceTransitionTime);
        }
        else
        {
            //Debug.Log("Night forest");
            nightForest.TransitionTo(ambienceTransitionTime);
        }
    }

    void PlayTwilight()
    // choose which twilight ambience to play
    {
        if (city)
        {
            //Debug.Log("Twilight city");
            sunsetCity.TransitionTo(ambienceTransitionTime);
        }
        else
        {
            //Debug.Log("Twilight forest");
            sunsetForest.TransitionTo(ambienceTransitionTime);
        }
    }

    void PlayDay()
    // choose which day ambience to play
    {
        if (city)
        {
            //Debug.Log("Day city");
            dayCity.TransitionTo(ambienceTransitionTime);
        }
        else
        {
            //Debug.Log("Day forest");
            dayForest.TransitionTo(ambienceTransitionTime);
        }
    }

    void CheckState()
    {
        state = CalculateState(time);

        switch (state)
        {
            case 0:
                PlayNight();
                break;
            case 1:
                PlayTwilight();
                break;
            case 2:
                PlayDay();
                break;
        }
    }

    public IEnumerator TimeCheck()
    {
        while(checkTime)
        {
            UpdateTime();
            if(CalculateState(time) != state || city != oldCity || updateRequired)
            {
                updateRequired = false;
                oldCity = city;
                CheckState();
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
