using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U : object {

    public static List <GameObject> FindNearestBuildings (Vector3 position, float radius )
    {
        List<GameObject> returnObjects = new List<GameObject>();
        int layerMask = 1 << 8;

        Collider[] hitColliders = Physics.OverlapSphere(position, radius, layerMask);

        if (hitColliders.Length != 0)
        {
            foreach (Collider hitcol in hitColliders)
            {
                returnObjects.Add(hitcol.gameObject);
            }
        }
        return returnObjects;
    }

    public static List<ResidentialTracker> ReturnResidentialTrackers(List<GameObject> objectList)
    {
        List<ResidentialTracker> returnObject = new List<ResidentialTracker>();
        for(int i = 0; i < objectList.Count; i++)
        {
            if(objectList[i].tag == "residential")
            {
                returnObject.Add(objectList[i].GetComponent<ResidentialTracker>());
            }
        }

        return returnObject;
    }

    public static List<IndustrialTracker> ReturnIndustrialTrackers(List<GameObject> objectList)
    {
        List<IndustrialTracker> returnObject = new List<IndustrialTracker>();
        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].gameObject.tag == "industrial")
            {
                returnObject.Add(objectList[i].GetComponent<IndustrialTracker>());
            }
        }

        return returnObject;
    }

    public static List<CommercialTracker> ReturnCommercialTrackers(List<GameObject> objectList)
    {
        List<CommercialTracker> returnObject = new List<CommercialTracker>();
        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].tag == "commercial")
            {
                returnObject.Add(objectList[i].GetComponent<CommercialTracker>());
            }
        }

        return returnObject;
    }

    public static List<LeisureTracker> ReturnLeisureTrackers(List<GameObject> objectList)
    {
        List<LeisureTracker> returnObject = new List<LeisureTracker>();
        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].tag == "industrial")
            {
                returnObject.Add(objectList[i].GetComponent<LeisureTracker>());
            }
        }

        return returnObject;
    }

    public static string IntToTag(int value)
    {
        Dictionary<int, string> type = new Dictionary<int, string>();

        type.Add(0, "residential");
        type.Add(1, "commercial");
        type.Add(2, "industrial");
        type.Add(3, "commercial");
        type.Add(4, "industrialComponent");
        type.Add(5, "foliage");

        Debug.Log("typecount : " + type.Count);
        return type[value];
    }

    public static bool CompareStrings(string currentString, string requirementString)
    {
        bool completed = true;
        for(int i = 0; i < requirementString.Length; i++)
        {
            if(requirementString[i] == "1"[0])
            {
                if(currentString[i] == "0"[0])
                {
                    completed = false;
                }
            }
        }
        return completed;
    }

    public static void LeftPulse()
    {
        SteamVR_Controller.Input(1).TriggerHapticPulse(500);
    }

    public static void LeftPulse(int time)
    {
        SteamVR_Controller.Input(1).TriggerHapticPulse((ushort)time);
    }

    public static void RightPulse()
    {
        SteamVR_Controller.Input(2).TriggerHapticPulse(500);
    }

    public static void RightPulse(int time)
    {
        SteamVR_Controller.Input(2).TriggerHapticPulse((ushort)time);
    }
}
