using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

public class BuildingNotificationTracker : MonoBehaviour {

    bool updating;
    BuildingNotificationManager manager;
    ItemTracker tracker;
    float time;

    public Sprite noPower;
    public Sprite noUsers;
    public Sprite noSales;
    public Sprite noProduction;
    public Sprite lowHappiness;
    public Sprite highUnemployment;
    public Sprite imageSeven;
    public Sprite imageEight;

    Notification displayingNotification;
    GameObject notificationPrefab;

	// Use this for initialization
	void Start () {if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
        manager = ReferenceManager.instance.buildingNotificationManager;
        BuildingNotificationManager.notificationUpdater += CheckConditions;
        tracker = GetComponent<ItemTracker>();
        notificationPrefab = ReferenceManager.instance.notificationPrefab;
	}
	
	void CheckConditions()
    // Check against conditions
    {
        if(tracker.updateStarted)
        {
            if (HighUnemployment() && !updating)
            {
                updating = true;
                GameObject notificationObject = Instantiate(notificationPrefab, transform.position, Quaternion.identity);
                notificationObject.transform.parent = transform;
                displayingNotification = new Notification(5, highUnemployment, notificationObject, transform.position + new Vector3(0f, 3f, 0f));
                RunNotification(displayingNotification);
            }
            else if (NoProduction() && !updating)
            {
                updating = true;
                GameObject notificationObject = Instantiate(notificationPrefab, transform.position, Quaternion.identity);
                notificationObject.transform.parent = transform;
                displayingNotification = new Notification(2, noProduction, notificationObject, transform.position + new Vector3(0f, 3f, 0f));
                RunNotification(displayingNotification);
            }
            else if (NoSales() && !updating)
            {
                updating = true;
                GameObject notificationObject = Instantiate(notificationPrefab, transform.position, Quaternion.identity);
                notificationObject.transform.parent = transform;
                displayingNotification = new Notification(3, noSales, notificationObject, transform.position + new Vector3(0f, 3f, 0f));
                RunNotification(displayingNotification);
            }
            else if (NoPower() && !updating)
            {
                updating = true;
                GameObject notificationObject = Instantiate(notificationPrefab, transform.position, Quaternion.identity);
                notificationObject.transform.parent = transform;
                displayingNotification = new Notification(4, noPower, notificationObject, transform.position + new Vector3(0f, 3f, 0f));
                RunNotification(displayingNotification);
            }
            else if (LowHappiness() && !updating)
            {
                updating = true;
                GameObject notificationObject = Instantiate(notificationPrefab, transform.position, Quaternion.identity);
                notificationObject.transform.parent = transform;
                displayingNotification = new Notification(5, lowHappiness, notificationObject, transform.position + new Vector3(0f, 3f, 0f));
                RunNotification(displayingNotification);
            }
            else if (NoUsers() && !updating)
            {
                updating = true;
                GameObject notificationObject = Instantiate(notificationPrefab, transform.position, Quaternion.identity);
                notificationObject.transform.parent = gameObject.transform;
                displayingNotification = new Notification(1, noUsers, notificationObject, transform.position + new Vector3(0f, 3f, 0f));
                RunNotification(displayingNotification);
            }
        }
    }

    bool IndividualCondition(int id)
    // Runs single condition function by id
    {
        if (id == 1)
            return NoUsers();
        else if (id == 2)
            return NoProduction();
        else if (id == 3)
            return NoSales();
        else if (id == 4)
            return NoPower();
        else if (id == 5)
            return LowHappiness();
        else if (id == 6)
            return HighUnemployment();
        else return false;
    }

    void RunNotification(Notification notification)
    // Begins displaying notification
    {
        updating = true;
        notification.SetPosition(transform.position);
        time = 0;

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "CheckNotification");
    }

    void CheckRemove(int id)
    // Check if notification should still be displayed
    {
        if(!IndividualCondition(id))
        {
            RemoveNotification();
        }
    }

    void RemoveNotification()
    {
        updating = false;
        displayingNotification.DestroyNotification();
    }

    IEnumerator CheckNotification()
    {
        while(updating)
        {
            if (time >= 10)
            {
                RemoveNotification();
                time = 0;
                break;
            }
            CheckRemove(displayingNotification.id);
            time += Time.deltaTime;
            yield return null;
        }
    }


    // Condition definition

    bool NoUsers()
    {
        if (tracker.users == 0)  // Replace with condition here
            return true;
        else return false;
    }

    bool NoSales()
    {
        if (tracker.type == "commercial" && tracker.goodsSold == 0)
            return true;
        else return false;
    }

    bool NoProduction()
    {
        if (tracker.type == "industrial" && tracker.goodsProduced == 0)
            return true;
        else return false;
    }

    bool LowHappiness()
    {
        if (tracker.currentHappiness < 20)
            return true;
        else return false;
    }

    bool NoPower()
    {
        return tracker.power;
    }

    bool HighUnemployment()
    {
        if (tracker.unemployedPopulation != 0)
        {
            if (tracker.users / tracker.unemployedPopulation <= 5)
                return true;
            else return false;
        }
        else return false;
    }

    protected class Notification
    {
        public int id;

        Sprite image;

        SpriteRenderer notificationRend;
        GameObject notificationObject;

        public Notification(int newId, Sprite newSprite, GameObject newNotificationObject, Vector3 newPosition)
        {
            id = newId;
            image = newSprite;
            notificationObject = newNotificationObject;
            notificationObject.transform.position = newPosition;
            notificationRend = notificationObject.GetComponent<SpriteRenderer>();
            notificationRend.sprite = image;
            notificationObject.GetComponent<Animator>().enabled = true;
        }

        public void SetNewNotification(int newId, Sprite newSprite)
        {
            id = newId;
            image = newSprite;
            notificationRend.sprite = image;
        }

        public void DestroyNotification()
        // Stops coroutine and destroys notification
        {
            Destroy(notificationObject);
        }

        public void SetPosition(Vector3 newPosition)
        // Sets position, 
        {
           notificationObject.transform.position = newPosition += new Vector3(0f, 3f, 0f);
        }
    }
}
