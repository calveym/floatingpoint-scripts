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
            Debug.Log(sg.Name);
            if (sg.Name == slot)
            {
                LevelSerializer.LoadSavedLevel(sg.Data);
            }
        }
    }

    public void SaveSlotOne()
    {
        Debug.Log("Saving slot 1");
        Save("slot1");
    }

    public void SaveSlotTwo()
    {
        Save("slot2");
    }

    public void SaveSlotThree()
    {
        Save("slot3");
    }

    public void LoadSlotThree()
    {
        Load("slot3");
    }

    public void LoadSlotTwo()
    {
        Load("slot2");
    }

    public void LoadSlotOne()
    {
        Debug.Log("Loading slot 1");
        Load("slot1");
    }

    public void LoadEmpty()
    {
        SceneManager.LoadScene("_final-0.5.0-empty");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("_final-0.5.0-tutorial");
    }
}
