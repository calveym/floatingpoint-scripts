using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Autelia.Serialization;

public class U : MonoBehaviour {

    public static U instance;
    public AudioClip click;
    SteamVR_Controller.Device rightDevice;
    SteamVR_Controller.Device leftDevice;

    private void Awake()
    {
        if(instance != this)
        {
            instance = this;
        }
    }

    private void Start()
    {if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
        int rightIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        int leftIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);

        rightDevice = SteamVR_Controller.Input(rightIndex);
        leftDevice = SteamVR_Controller.Input(leftIndex);

        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn += LeftGrab;
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn += RightGrab;
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff += LeftGrab;
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff += RightGrab;
    }
    
    public void LeftGrab(object sender, ControllerInteractionEventArgs e)
    {
        LeftPulse();
    }

    public void RightGrab(object sender, ControllerInteractionEventArgs e)
    {
        RightPulse();
    }

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

    public static List<GameObject> FindNearestRoads(Vector3 position, float radius)
    {
        List<GameObject> returnObject = new List<GameObject>();

        int layerMask = 1 << 11;

        Collider[] hitColliders = Physics.OverlapSphere(position, radius, layerMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].tag == "road")
            {
                returnObject.Add(hitColliders[i].gameObject);
            }
        }

        return returnObject;
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

    public static int NumResidents(List<ResidentialTracker> allBuildings)
    {
        int returnValue = 0;
        foreach(ResidentialTracker res in allBuildings)
        {
            returnValue += res.users;
        }
        return returnValue;
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

        //Debug.Log("typecount : " + type.Count);
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
        LeftPulse(750);
    }

    public static void LeftPulse(int time)
    {
        if (instance.leftDevice != null)
            instance.leftDevice.TriggerHapticPulse((ushort)time);
    }

    public static void RightPulse()
    {
        RightPulse(750);
    }

    public static void RightPulse(int time)
    {
        if (instance.rightDevice != null)
            instance.rightDevice.TriggerHapticPulse((ushort)time);
    }

    public static void DisablePhysics(GameObject disableObject)
    {
        Rigidbody rb = disableObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public static void EnablePhysics(GameObject enableObject)
    {
        Rigidbody rb = enableObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public static void PlayClick()
    {
        ReferenceManager.instance.audioManager.PlaySingle(instance.click);
    }

    public static void LerpValue(float floatToLerp, float endValue, float lerpTime = 1f)
    {
        float startValue = floatToLerp;
        instance.LerpValue(floatToLerp, startValue, endValue, lerpTime);
    }

    private void LerpValue(float floatToLerp, float startValue, float endValue, float lerpTime)
    {

Autelia.Coroutines.CoroutineController.StartCoroutine(instance.GradualLerp(floatToLerp, startValue, endValue, lerpTime));
    }

    IEnumerator GradualLerp(float lerpingFloat, float startValue, float endValue, float time)
    {
        float totalTime = 0;
        while(totalTime < time)
        {
            totalTime += Time.deltaTime;
            lerpingFloat = Mathf.Lerp(startValue, endValue, totalTime / time);
            yield return null;
        }
    }
}
