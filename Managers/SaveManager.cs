using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;
using UnityEngine.SceneManagement;


public class SaveManager : MonoBehaviour {

    public SerializerHook hook;

    void Save(string slot)
    {
        hook.Save(slot);
    }

    void Load(string slot)
    {
        hook.Load(slot);
    }


    public void SaveSlotOne()
    {
        if (Time.timeSinceLevelLoad > 2)
        {
            Debug.Log("Saving slot 1...");
            Save("slot1");
        }
    }

    public void SaveSlotTwo()
    {
        if (Time.timeSinceLevelLoad > 2)
        {
            Debug.Log("Saving slot 2...");

            Save("slot2");
        }
    }

    public void SaveSlotThree()
    {
        if (Time.timeSinceLevelLoad > 2)
        {
            Debug.Log("Saving slot 3...");

            Save("slot3");
        }
    }

    public void LoadSlotThree()
    {
        if (Time.timeSinceLevelLoad > 2)
        {
            Debug.Log("Loading slot 3...");

            Load("slot3");
        }
    }

    public void LoadSlotTwo()
    {
        if (Time.timeSinceLevelLoad > 2)
        {
            Debug.Log("Loading slot 2...");

            Load("slot2");
        }
    }

    public void LoadSlotOne()
    {
        if (Time.timeSinceLevelLoad > 2)
        {
            Debug.Log("Loading slot 1");

            Load("slot1");
        }
    }

    public void LoadEmpty()
    {
        if (Time.timeSinceLevelLoad > 2)
        {
            SceneManager.LoadScene("1.0.0-empty");
        }
    }

    public void LoadTutorial()
    {
        if (Time.timeSinceLevelLoad > 2)
        {
            SceneManager.LoadScene("1.0.0-tutorial");
        }
    }
}
