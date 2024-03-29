using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

public class TutorialTracker : MonoBehaviour {

    [Range(0f, 15f )]
    [Tooltip("Level for tutorial progression")]
    public int trackerLevel;

    [Tooltip("Material to swap to on tutorial reach")]
    public Material completionMaterial;


    TutorialManager manager;
    MeshRenderer rend;

    private void Awake()
    {

    }

	// Use this for initialization
	void Start () {if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
        manager = GameObject.Find("Managers").GetComponent<TutorialManager>();
        manager.AddSphere(this);
        rend = GetComponent<MeshRenderer>();
        gameObject.SetActive(false);
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

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "DoEntry");
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
        Destroy(gameObject);
    }
}
