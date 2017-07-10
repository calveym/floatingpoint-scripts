using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

public class ProgressionManager : MonoBehaviour {

    static ProgressionManager instance;

    DisplayMenu displayMenu;
    PopulationManager populationManager;
    PopupManager popupManager;

    public int level = 0;
    public bool allowLevelUp;
    public AudioClip levelUpSound;

    Dictionary<int, int> levelReq; // Requirement for population to level up

    public delegate void LevelUp();

    LevelUp levelUp;

    public void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        popupManager = GameObject.Find("Managers").GetComponent<PopupManager>();
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        levelReq = new Dictionary<int, int>();
        levelReq.Add(1, 10);
        levelReq.Add(2, 25);
        levelReq.Add(3, 50);
        levelReq.Add(4, 100);
        levelReq.Add(5, 175);
        levelReq.Add(6, 225);
        levelReq.Add(7, 300);
        levelReq.Add(8, 500);
        levelReq.Add(9, 750);
        levelReq.Add(10, 1000);
        levelReq.Add(11, 1500);
        levelReq.Add(12, 2000);
        levelReq.Add(13, 3000);
        levelReq.Add(14, 5000);
        levelReq.Add(15, 7500);
        levelReq.Add(16, 100000);
        Autelia.Coroutines.CoroutineController.StartCoroutine(this, "SlowUpdate");
    }
    
    void CheckLevelUp()
    {
        // Perform check to see whether next level that returns false from levelInfo can be completed.
        int currentLevelReq;
        levelReq.TryGetValue(level + 1, out currentLevelReq);
        if(populationManager.totalPopulation >= currentLevelReq)
        {
            PerformLevelUp(level + 1);
        }
    }

    void PerformLevelUp(int newLevel)
    {
        if (levelUp != null)
            levelUp();
        AudioManager.instance.PlaySingle(levelUpSound);
        level = newLevel;
        string levelUpString = string.Format("Level up! You are now Level {0} You have unlocked some new buildings!",
                                         level);
        PopupManager.Popup(levelUpString);
    }

    void UnlockBuildingTier()
    {
        displayMenu.SetTier(level + 1);
    }
    
    public void AllowRemoveMountains()
    {
        GameObject[] mountains = GameObject.FindGameObjectsWithTag("mountain");
        for (int i = 0; i < mountains.Length; i++)
        {
            mountains[i].GetComponent<ProgressionTracker>().Enable();
        }
    }

    public string ToNextLevel()
    {
        int currentLevelReq;
        instance.levelReq.TryGetValue(level + 1, out currentLevelReq);
        return (currentLevelReq - instance.populationManager.population).ToString();
    }

    IEnumerator SlowUpdate()
    {
        while(true)
        {
            if(allowLevelUp)
            {
                CheckLevelUp();
            }
            yield return new WaitForSeconds(1);
        }
    }
}
