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
        QueuePopup("Welcome to CloudCity!");
        QueuePopup("Touch the left trackpad to open the building menu");
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
            //tooltip.UpdateText(queuedPopups[0]);
            popup.SetActive(true);
            queuedPopups.RemoveAt(0);
            yield return new WaitForSeconds(WAIT_TIME);
        }
        if (queuedPopups.Count == 0)
        {
            popup.SetActive(false);
            running = false;
            yield return null;
        }
    }
}
