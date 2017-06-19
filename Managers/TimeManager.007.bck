using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public GameObject headlamp;

    private void Start()
    {
        StartCoroutine("CheckLamp");
    }

    IEnumerator CheckLamp()
    {
        while(true)
        {
            if (TOD_Sky.Instance.IsNight && !headlamp.activeSelf)
            {
                headlamp.SetActive(true);
            }
            else if(TOD_Sky.Instance.IsDay && headlamp.activeSelf)
                headlamp.SetActive(false);
            yield return new WaitForSeconds(1);
        }
    }
}
