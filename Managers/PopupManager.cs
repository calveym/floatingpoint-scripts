using System.Collections;
using VRTK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Autelia.Serialization;
using Autelia.Serialization;

public class PopupManager : MonoBehaviour {

    static PopupManager instance;
    public bool showUI = true;
    public GameObject popup;
    Text tooltip;
    public AudioClip notificationSound;

    List<string> queuedPopups;
    float WAIT_TIME;
    bool running = false;

    private void Awake()
    {if (Serializer.IsLoading)	return;
        instance = this;
        queuedPopups = new List<string>();
    }

    void Start()
    {if (Serializer.IsLoading)	return;
        WAIT_TIME = 10f;
        tooltip = popup.transform.Find("TooltipCanvas/UIContainer/UITextFront").GetComponent<Text>();
        popup.SetActive(false);
        QueuePopup("Welcome to CloudCity! It's time to start building your city!\n Fly over to the blue orb by touching the top half of the right trackpad to begin");
    }

    public static void Popup(string message)
    {
        instance.QueuePopup(message);
    }

	void QueuePopup(string message)
    {
        queuedPopups.Add(message);
        if(!running && showUI)
        {

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "DoPopup");
        }
    }

    IEnumerator DoPopup()
    // Goes through array of queued popups, with a WAIT_TIME (5s) delay
    {
        running = true;
        yield return new WaitForSeconds(5);
        while (queuedPopups.Count > 0)
        {
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
