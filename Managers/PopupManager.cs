﻿using System.Collections;
using VRTK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {

    public GameObject popup;
    Text tooltip;
    public AudioClip notificationSound;

    List<string> queuedPopups;
    float WAIT_TIME;
    bool running;

    void Start()
    {
        WAIT_TIME = 5;
        queuedPopups = new List<string>();
        tooltip = popup.transform.Find("TooltipCanvas/UIContainer/UITextFront").GetComponent<Text>();
        QueuePopup("Welcome to your town! Let's go have a closer look at those buildings-");
        QueuePopup("Fly over to the blue orb with the right trackpad");
    }

    public static void Popup(string message)
    {
        GameObject.Find("Managers").GetComponent<PopupManager>().QueuePopup(message);
    }

	void QueuePopup(string message)
    {
        queuedPopups.Add(message);
        if(!running)
        {
            StartCoroutine("DoPopup");
        }
    }

    IEnumerator DoPopup()
    // Goes through array of queued popups, with a WAIT_TIME (5s) delay
    {
        running = true;
        while (queuedPopups.Count > 0)
        {
            AudioSource.PlayClipAtPoint(notificationSound, tooltip.gameObject.transform.position);
            tooltip.text = queuedPopups[0];
            popup.SetActive(true);
            queuedPopups.RemoveAt(0);
            yield return new WaitForSeconds(WAIT_TIME);
        }
        if (queuedPopups.Count == 0)
        {
            Debug.Log("Popups all shown, deactivating");
            popup.SetActive(false);
            running = false;
            yield return null;
        }
    }
}
