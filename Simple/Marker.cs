using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {

    float rotating;
    float spinSpeed;

    private void Start()
    {
        spinSpeed = 180f;
    }

    public void StartRotation()
    {
        rotating = 0;
        StartCoroutine("Rotate", gameObject);
    }

    IEnumerator Rotate(GameObject rotateObject)
    // Smoothly rotates object
    {
        while(rotating <= 5)
        {
            rotating += Time.deltaTime;
            transform.Rotate(new Vector3(0f, spinSpeed * Time.deltaTime, 0f));
            yield return null;
        }
    }
}
