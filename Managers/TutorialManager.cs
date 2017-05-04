using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TutorialManager : MonoBehaviour {

    VRTK_ControllerEvents leftEvents;
    VRTK_ControllerEvents rightEvents;

    public AudioClip rewardEntry;
    public static int tutorialProgress;
    bool continueTutorial;

    Dictionary<int, string> tutorialStrings;
    Dictionary<int, int> levelReq; // Requirement for population to level up
    List<string> tutorialRequirements;


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
    {
        tutorialProgress = 0;
        tutorialStrings = new Dictionary<int, string>();
        tutorialRequirements = new List<string>();
        continueTutorial = true;
        tutorialRequirements.Add("000100001");
        tutorialRequirements.Add("100000000");
        tutorialRequirements.Add("100000000");
        tutorialRequirements.Add("000010000");

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
    }

    // Use this for initialization
    void Start () {
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
        StartCoroutine("TutorialChecker");
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

    void CheckTutorialProgress()
    {
        if(U.CompareStrings(GetStateString(), tutorialRequirements[tutorialProgress]))
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

            tutorialProgress++;
            // Tutorial progress complete here
            //ProgressionManager.TryLevelUp();
        }
    }

    int GetLevelReq()
    {
        int currentLevelReq;
        levelReq.TryGetValue(tutorialProgress + 1, out currentLevelReq);
        return currentLevelReq;
    }

    string GetStateString()
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
    {
        tutorialStrings.Add(0, "Great! Now it's time to add in our first building. \n Click the left trackpad to open up the buildings menu");
        tutorialStrings.Add(1, "Well done. You can swipe between the building types by swiping on the left trackpad. \n Try that now, then press the left trackpad to select Residential");
        tutorialStrings.Add(2, "These are residential buildings. Select a building by scrolling with your left trackpad.\n Once a valid building is selected, pull the left trigger to purchase");
        tutorialStrings.Add(3, "Your people need more places to work! Let's add a small shop nearby. Open up the building menu again, and add a commercial building");
        tutorialStrings.Add(4, "Good work. You can also keep a closer eye on the development of individual buildings by pressing the right menu button");
        tutorialStrings.Add(6, "Let's upgrade it with some industrial components. Open up the building panel, and add one in now.");
        tutorialStrings.Add(7, "You can check the effects of each component by viewing the building stats tooltip with the right menu button");

    }

    IEnumerator TutorialChecker()
    {
        while(true)
        {
            CheckTutorialProgress();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
