using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preload : MonoBehaviour {
	public void Awake () {
        Debug.Log("DDOL running");
        DontDestroyOnLoad(gameObject);
        if (Autelia.Serialization.Serializer.IsLoading) return;
        UnityEngine.SceneManagement.SceneManager.LoadScene("1.0.0-tutorial");
    }
}
