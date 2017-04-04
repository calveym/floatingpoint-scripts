using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            if (sg.Caption == slot)
            {
                LevelSerializer.LoadSavedLevel(sg.Data);
            }
        }
    }

    public void SaveSlotOne()
    {
        Debug.Log("Saving...");  
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
        Debug.Log(LevelSerializer.SavedGames["test"]);
    }
}
