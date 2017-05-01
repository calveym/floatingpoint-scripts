using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTracker : MonoBehaviour {

    [Range(0f, 15f )]
    [Tooltip("Level for tutorial progression")]
    public int trackerLevel;

    [Tooltip("Material to swap to on tutorial reach")]
    public Material completionMaterial;


    TutorialManager manager;
    MeshRenderer rend;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("Managers").GetComponent<TutorialManager>();
        rend = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(trackerLevel == TutorialManager.tutorialProgress + 1 &&( other.name == "Headset" || other.name == "LeftController" || other.name == "RightController"))
        {
            SendTriggerEntry();
        }
    }

    void SendTriggerEntry()
    {
        manager.TutorialTriggerEnter(trackerLevel, this);
        StartCoroutine("DoEntry");
    }

    IEnumerator DoEntry()
    {
        rend.material = completionMaterial;
        float time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            transform.localScale *= 1.001f;
            yield return null;
        }
    }
}
