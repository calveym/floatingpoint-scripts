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
        if (other.name == "Head" ||
            other.name == "SideA" ||
            other.name == "Controller(right)" ||
            other.name == "RightController" ||
            other.name == "Headset")
        {
            SendTriggerEntry();
        }
    }

    void SendTriggerEntry()
    {
        StartCoroutine("DoEntry");
        manager.TutorialTriggerEnter(trackerLevel, this);
    }

    IEnumerator DoEntry()
    {
        rend.material = completionMaterial;
        float time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return null;
        }
    }
}
