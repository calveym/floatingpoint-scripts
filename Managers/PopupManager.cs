using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour {

    GameObject tooltip;
    public Text tooltipText;

    List<string> queuedPopups;
    float WAIT_TIME;

    void Start()
    {
        WAIT_TIME = 5;
    }

	void QueuePopup(string message)
    {
        queuedPopups.Add(message);
        StartCoroutine("DoPopup");
    }

    IEnumerator DoPopup()
    // Goes through array of queued popups, with a WAIT_TIME delay
    {
        if(queuedPopups.Count == 0)
        {
            tooltip.SetActive(false);
            yield return null;
        }
        while(queuedPopups.Count > 0)
        {
            tooltipText.text = queuedPopups[0];
            tooltip.SetActive(true);
            queuedPopups.RemoveAt(0);
            yield return WAIT_TIME;
        }
    }
}
