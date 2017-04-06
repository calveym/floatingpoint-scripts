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
}
