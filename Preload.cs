using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preload : MonoBehaviour {
	public void Awake () {
        Debug.Log("DDOL running");
        DontDestroyOnLoad(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("_main-0.5.1");

    }
}
