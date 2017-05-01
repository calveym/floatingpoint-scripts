using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TutorialManager : MonoBehaviour {

    VRTK_ControllerEvents leftEvents;
    VRTK_ControllerEvents rightEvents;

    AudioClip rewardEntry;
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
        tutorialStrings = new Dictionary<int, string>();
        tutorialRequirements = new List<string>();
        continueTutorial = true;
        tutorialRequirements.Add("001100000");
        tutorialRequirements.Add("000000001");

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
        tutorialProgress = 0;

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
        Debug.Log("Touch - L");
        leftTouchpadTouch = true;
    }

    void DoRightTouchpadTouch(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("Touch - R");
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
            PopupManager.Popup(tutorialStrings[tutorialProgress]);
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

            PopupManager.Popup("Tutorial stage completed, reach " + GetLevelReq() + " population to level up");

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
        tutorialStrings.Add(0, "Great! Now, let's move some of these homes closer to the shops so your citizens can find work!");
        tutorialStrings.Add(1, "Good work. Now open up the global info panel by touching the left trackpad.");
        tutorialStrings.Add(2, "You can view more detailed building stats by pressing the right menu button");
        tutorialStrings.Add(3, "Your people need more places to work! Let's add an industrial building just out of the town.");
        tutorialStrings.Add(4, "Open up the buildings panel by pressing the left trackpad, and select an industrial building");
        tutorialStrings.Add(5, "You level up as your population grows. Unlock more buildings and building types at higher levels");
        tutorialStrings.Add(6, "It looks like that industrial area is doing pretty well, but it could be better.");
        tutorialStrings.Add(7, "Let's upgrade it with some industrial components. Open up the building panel, and add one in now.");
        tutorialStrings.Add(8, "You can check the effects of each component by viewing the building stats tooltip with the right menu button");

    }

    IEnumerator TutorialChecker()
    {
        while(true)
        {
            CheckTutorialProgress();
            yield return new WaitForSeconds(1f);
        }
    }
}
