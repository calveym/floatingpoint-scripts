using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Autelia.Serialization;
using Autelia.Serialization;

public class TutorialManager : MonoBehaviour {

    VRTK_ControllerEvents leftEvents;
    VRTK_ControllerEvents rightEvents;

    public bool checkTutorial;
    public AudioClip rewardEntry;
    [Range(0, 15)]
    public int tutorialProgress = 0;

    Dictionary<int, string> tutorialStrings;  // Strings for each level, printed via popup manager
    Dictionary<int, int> levelReq;  // Requirement for population to level up
    List<string> tutorialRequirements;  // Requirements for each tutorial level
    Dictionary<int, TutorialTracker> spheres;  // Stores sphere for each tutorial level


    // Booleans for checking requirements
    bool leftTouchpadPress;
    bool rightTouchpadPress;
    bool leftTouchpadTouch;
    bool rightTouchpadTouch;
    bool leftTriggerPress;
    bool rightTriggerPress;
    bool leftMenu;
    bool rightMenu;
    bool triggerEntry;

    private void Awake()
    {if (Serializer.IsLoading)	return;
        spheres = new Dictionary<int, TutorialTracker>();
        tutorialStrings = new Dictionary<int, string>();
        tutorialRequirements = new List<string>();
        tutorialRequirements.Add("000100001");
        tutorialRequirements.Add("101000000");
        tutorialRequirements.Add("100000000");
        tutorialRequirements.Add("100000000");
        tutorialRequirements.Add("001000000");
        tutorialRequirements.Add("101000001");
        tutorialRequirements.Add("000000010");
        tutorialRequirements.Add("000000100");
    }

    // Use this for initialization
    void Start () {if (Serializer.IsLoading)	return;
        leftEvents = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();
        rightEvents = GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>();

        leftEvents.TouchpadPressed += DoLeftTouchpadPress;
        rightEvents.TouchpadPressed += DoRightTouchpadPress;

        leftEvents.TouchpadTouchStart += DoLeftTouchpadTouch;
        rightEvents.TouchpadTouchStart += DoRightTouchpadTouch;

        leftEvents.TriggerClicked += DoLeftTriggerPress;
        rightEvents.TriggerClicked += DoRightTriggerPress;

        leftEvents.AliasMenuOn += DoLeftMenuPress;
        rightEvents.AliasMenuOn += DoRightMenuPress;

        SetupTutorial();

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "TutorialChecker");
	}

    void DoLeftTouchpadPress(object sender, ControllerInteractionEventArgs e)
    {
        leftTouchpadPress = true;
    }

    void DoRightTouchpadPress(object sender, ControllerInteractionEventArgs e)
    {
        rightTouchpadPress = true;
    }

    void DoLeftTouchpadTouch(object sender, ControllerInteractionEventArgs e)
    {
        leftTouchpadTouch = true;
    }

    void DoRightTouchpadTouch(object sender, ControllerInteractionEventArgs e)
    {
        rightTouchpadTouch = true;
    }

    void DoLeftTriggerPress(object sender, ControllerInteractionEventArgs e)
    {
        leftTriggerPress = true;
    }

    void DoRightTriggerPress(object sender, ControllerInteractionEventArgs e)
    {
        rightTriggerPress = true;
    }

    void DoLeftMenuPress(object sender, ControllerInteractionEventArgs e)
    {
        leftMenu = true;
    }

    void DoRightMenuPress(object sender, ControllerInteractionEventArgs e)
    {
        rightMenu = true;
    }

    public void TutorialTriggerEnter(int senderID, TutorialTracker sender)
    // Called from tutorial tracker sphere
    {
        if(senderID == tutorialProgress)
        {
            AudioSource.PlayClipAtPoint(rewardEntry, sender.transform.position);
            triggerEntry = true;
        }
    }

    public void SendNewLevel(int newLevel)
    {
        tutorialProgress = newLevel;
    }

    public void AddSphere(TutorialTracker tracker)
    {
        TutorialTracker check;
        spheres.TryGetValue(tracker.trackerLevel, out check);
        if(!check)
        {
            spheres.Add(tracker.trackerLevel, tracker);
        }
    }

    void CheckTutorialProgress()
    // Checks to see if tutorial is completed
    {
        SphereCheck();

        if (U.CompareStrings(GetStateString(), tutorialRequirements[tutorialProgress]))
        {
            leftTouchpadPress = false;
            rightTouchpadPress = false;
            leftTouchpadTouch = false;
            rightTouchpadTouch = false;
            leftMenu = false;
            rightMenu = false;
            leftTriggerPress = false;
            rightTriggerPress = false;

            PopupManager.Popup(tutorialStrings[tutorialProgress]);

            //PopupManager.Popup("Tutorial stage " + tutorialProgress + " completed, reach " + GetLevelReq() + " population to level up and continue");
            // Tutorial progress complete here

            tutorialProgress++;
            
            //ProgressionManager.TryLevelUp();
        }
    }

    void SphereCheck()
    // Checks if next level has a sphere
    {
        TutorialTracker check;
        spheres.TryGetValue(tutorialProgress, out check);
        if(check)
        // Level has a sphere
        {
            EnableSphere(check);
        }
    }

    void EnableSphere(TutorialTracker sphere)
    // Called if tutorial level has a sphere
    {
        sphere.gameObject.SetActive(true);
    }

    int GetLevelReq()
    {
        int currentLevelReq;
        levelReq.TryGetValue(tutorialProgress + 1, out currentLevelReq);
        return currentLevelReq;
    }

    string GetStateString()
    // Wheeeee! if if if if if if if if if if......
    {
        string returnString = "";
        if (leftTouchpadPress)
        {
            returnString += "1";
        }
        else returnString += "0";
        if (rightTouchpadPress)
        {
            returnString += "1";
        }
        else returnString += "0";
        if (leftTouchpadTouch)
        {
            returnString += "1";
        }
        else returnString += "0";
        if (rightTouchpadTouch)
        {
            returnString += "1";
        }
        else returnString += "0";
        if (leftTriggerPress)
        {
            returnString += "1";
        }
        else returnString += "0";
        if (rightTriggerPress)
        {
            returnString += "1";
        }
        else returnString += "0";
        if (leftMenu)
        {
            returnString += "1";
        }
        else returnString += "0";
        if (rightMenu)
        {
            returnString += "1";
        }
        else returnString += "0";
        if (triggerEntry)
        {
            returnString += "1";
        }
        else returnString += "0";
        return returnString;
    }

    void SetupTutorial()
    // Use this for scripting strings
    {
        tutorialStrings.Add(0, "Great! Now it's time to add in our first building. \n Click the left trackpad to open up the buildings menu");
        tutorialStrings.Add(1, "Well done. Clicking the top half of the left trackpad selects or advances, and the bottom half goes back. \n Try selecting the first building category now.");
        tutorialStrings.Add(2, "These are residential buildings. Select a building by scrolling side to side with your left trackpad.\n Once a valid building is selected, press the top half of the left trackpad to purchase");
        tutorialStrings.Add(3, "Good job. Now, to move your new building, try grabbing it with either controller's bumper buttons.\n When you are ready to continue, exit the building UI.");
        tutorialStrings.Add(4, "Try viewing the global info panel by touching the left trackpad with your thumb.");
        tutorialStrings.Add(5, "Your citizens are unhappy! You can manage happiness by adding jobs, shops and landscaping. \n Try adding a shop in now (Second menu panel)");
        tutorialStrings.Add(6, "Great, now your citizens can find work! The smaller the shop, the closer it needs to be to the workers. \nYou can check on individual buildings' progress by pressing the right menu button.");
        tutorialStrings.Add(7, "Now it's time to explore what you've created! \n Press the left menu button to toggle between first person mode. \n Flight in first person is limited to walking height.");
        tutorialStrings.Add(8, "Finally, draw and delete roads with the right and left triggers respectively. \n Now it's your time to shine! Unlock new buildings by increasing your population.");
    }

    IEnumerator TutorialChecker()
    {
        while(checkTutorial)
        {
            CheckTutorialProgress();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
