using System.Collections;
using VRTK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {

    public GameObject popup;
    VRTK_ObjectTooltip tooltip;
    public AudioClip notificationSound;

    List<string> queuedPopups;
    float WAIT_TIME;
    bool running;

    void Start()
    {
        WAIT_TIME = 5;
        queuedPopups = new List<string>();
        tooltip = popup.GetComponent<VRTK_ObjectTooltip>();
        QueuePopup("Welcome to Neverland. Enjoy your stay.");
    }

	public void QueuePopup(string message)
    {
        queuedPopups.Add(message);
        if(!running)
        {
            StartCoroutine("DoPopup");
        }
    }

    IEnumerator DoPopup()
    // Goes through array of queued popups, with a WAIT_TIME delay
    {
        Debug.Log("Routine starting");
        running = true;
        if(queuedPopups.Count == 0)
        {
            popup.SetActive(false);
            running = false;
            yield return null;
        }
        while(queuedPopups.Count > 0)
        {
            AudioSource.PlayClipAtPoint(notificationSound, tooltip.gameObject.transform.position);
            popup.SetActive(true);
            queuedPopups.RemoveAt(0);
            yield return new WaitForSeconds(WAIT_TIME);
        }
    }
}
