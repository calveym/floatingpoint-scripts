using System.Collections;
using VRTK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {

    static PopupManager instance;
    public bool showUI = true;
    public GameObject popup;
    Text tooltip;
    public AudioClip notificationSound;

    List<string> queuedPopups;
    float WAIT_TIME;
    bool running = false;

    void Start()
    {
        if (instance == null)
            instance = this;
        WAIT_TIME = 10f;
        queuedPopups = new List<string>();
        tooltip = popup.transform.Find("TooltipCanvas/UIContainer/UITextFront").GetComponent<Text>();
        popup.SetActive(false);
        QueuePopup("Welcome to CloudCity! It's time to start building your city!\n Fly over to the blue orb by touching the top half of the right trackpad ");
    }

    public static void Popup(string message)
    {
        Debug.Log("Popping up");
        instance.QueuePopup(message);
    }

	void QueuePopup(string message)
    {
        queuedPopups.Add(message);
        if(!running && showUI)
        {
            Debug.Log("Popping in");
            StartCoroutine("DoPopup");
        }
    }

    IEnumerator DoPopup()
    // Goes through array of queued popups, with a WAIT_TIME (5s) delay
    {
        running = true;
        Debug.Log("Starting");
        while (queuedPopups.Count > 0)
        {
            Debug.Log("All the way thru");
            AudioManager.instance.PlaySingle(notificationSound);
            U.LeftPulse(1000);
            tooltip.text = queuedPopups[0];
            popup.SetActive(true);
            queuedPopups.RemoveAt(0);
            yield return new WaitForSeconds(WAIT_TIME);
        }
        if (queuedPopups.Count == 0)
        {
            //Debug.Log("Popups all shown, deactivating");
            popup.SetActive(false);
            running = false;
            yield return null;
        }
    }
}
