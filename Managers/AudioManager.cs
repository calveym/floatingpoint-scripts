using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public AudioSource efxSource;
    
    [Tooltip("How long it takes for ambiences to transition.")]
    public float ambienceTransitionTime;

    public AudioMixerSnapshot dayForest;
    public AudioMixerSnapshot dayCity;
    public AudioMixerSnapshot sunsetForest;
    public AudioMixerSnapshot sunsetCity;
    public AudioMixerSnapshot nightForest;
    public AudioMixerSnapshot nightCity;

    public static AudioManager instance = null;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    bool checkTime;

    // State 0 = Night
    // State 1 = Twilight
    // State 2 = Day

    int state;
    float time;
    bool city;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        checkTime = true;
        city = false;
        state = 0;
        StartCoroutine("TimeCheck");
    }

    public void PlaySingle(AudioClip clip)
    {
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

    void UpdateTime()
    {
         
    }

    int CalculateState(float newTime)
    {
        if (newTime > 0 && newTime < 5)
            return 0;
        else if (newTime >= 5 && newTime < 6.50)
            return 1;
        else if (newTime >= 6.50 && newTime < 17.5)
            return 2;
        else if (newTime >= 17.5 && newTime < 19)
            return 1;
        else if (newTime >= 19 && newTime < 24)
            return 0;
        else return 2;
    }

    void PlayNight()
    // choose which night ambience to play
    {
        if (city)
            nightCity.TransitionTo(ambienceTransitionTime);
        else
            nightForest.TransitionTo(ambienceTransitionTime);
    }

    void PlayTwilight()
    // choose which twilight ambience to play
    {
        if (city)
            sunsetCity.TransitionTo(ambienceTransitionTime);
        else
            sunsetForest.TransitionTo(ambienceTransitionTime);
    }

    void PlayDay()
    // choose which day ambience to play
    {
        if (city)
            dayCity.TransitionTo(ambienceTransitionTime);
        else
            dayForest.TransitionTo(ambienceTransitionTime);
    }

    public IEnumerator TimeCheck()
    {
        while(checkTime)
        {
            UpdateTime();
            if(CalculateState(time) != state)
            {
                // Update played sound, with fade (AudioMixerSnapshot.TransitionTo(float transitionTimeFam)
                switch(state)
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
                state = CalculateState(time);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
