using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour {

    void Save (string slot)
    {
        LevelSerializer.SaveGame(slot);
    }

    void Load(string slot)
    {
        foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
        {
            if (sg.Name == slot)
            {
                LevelSerializer.LoadSavedLevel(sg.Data);
            }
        }
    }

    public void SaveSlotOne()
    {
        if(Time.realtimeSinceStartup > 2)
        {
            Debug.Log("Saving slot 1...");
            Save("slot1");
        }
    }

    public void SaveSlotTwo()
    {
        if (Time.realtimeSinceStartup > 2)
        {
            Save("slot2");
        }
    }

    public void SaveSlotThree()
    {
        if (Time.realtimeSinceStartup > 2)
        {
            Save("slot3");
        }
    }

    public void LoadSlotThree()
    {
        if (Time.realtimeSinceStartup > 2)
        {
            Load("slot3");
        }
    }

    public void LoadSlotTwo()
    {
        if (Time.realtimeSinceStartup > 2)
        {
            Load("slot2");
        }
    }

    public void LoadSlotOne()
    {
        if (Time.realtimeSinceStartup > 2)
        {
            Debug.Log("Loading slot 1");
            Load("slot1");
        }
    }

    public void LoadEmpty()
    {
        if (Time.realtimeSinceStartup > 2)
        {
            SceneManager.LoadScene("main-0.6.0");
        }
    }

    public void LoadTutorial()
    {
        if (Time.realtimeSinceStartup > 2)
        {
            SceneManager.LoadScene("main-0.6.0");
        }
    }
}
