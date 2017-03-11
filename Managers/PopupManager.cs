using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // TODO
    {
        queuedPopups.Add(message);
        StartCoroutine(DoPopup);
    }

    IEnumerator DoPopup()
    {
        while(queuedPopups > 0)
        {
            tooltipText.text = queuedPopups[0];
            tooltip.SetActive(true);
            queuedPopups.RemoveAt(0);
            yield return WAIT_TIME;
        }
    }
}
