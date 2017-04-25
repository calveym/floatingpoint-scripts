using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUI : MonoBehaviour {

    GameObject staticSpheres;
    GameObject wheelBase;
    GameObject canvas;

    private void Awake()
    {
        staticSpheres = transform.Find("StaticSpheres").gameObject;
        wheelBase = transform.Find("WheelBase").gameObject;
        canvas = transform.Find("Canvas").gameObject;
    }

    private void Start()
    {
        HideUI();
    }
    
	public void ShowUI()
    {
        canvas.SetActive(true);
        wheelBase.SetActive(true);
        staticSpheres.SetActive(true);
    }

    public void HideUI()
    {
        canvas.SetActive(false);
        wheelBase.SetActive(false);
        staticSpheres.SetActive(false);
    }
}
