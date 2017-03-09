using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveManager : MonoBehaviour {

    public void Save()
    {
        Debug.Log("Saving...");
        LevelSerializer.SaveGame("test");
    }

    public void Load ()
    {
        Debug.Log(LevelSerializer.SavedGames["test"]);
    }
}
