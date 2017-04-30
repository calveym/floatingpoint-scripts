using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour {

    void Save (string slot)
    {
        // TEST
        LevelSerializer.SaveGame(slot);
    }

    void Load(string slot)
    {
        // TEST
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

    }

    public void SaveSlotThree()
    {

    }

    public void LoadSlotThree()
    {

    }

    public void LoadSlotTwo()
    {

    }

    public void LoadSlotOne()
    {
        Debug.Log("Loading slot 1");
        Load("slot1");
    }

    public void LoadEmpty()
    {
        SceneManager.LoadScene("main-0.5.0-empty");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("main-0.5.0-tutorial");
    }
}
