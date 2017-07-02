using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNotificationManager : MonoBehaviour {

    bool updateNotifications = true;

    public delegate void NotificationUpdater();
    public static NotificationUpdater notificationUpdater;

    private void Start()
    {

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "UpdateBuildingNotifications");
    }

    IEnumerator UpdateBuildingNotifications()
    {
        while(updateNotifications)
        {
            if(notificationUpdater != null)
            {
                //notificationUpdater();
            }
            yield return new WaitForSeconds(5.1f);
        }
    }
}
