using UnityEngine;
using System.Collections;

public class BuildingTooltipBase : TooltipBase
{

    protected ItemTracker tracker;

    protected int users;
    protected int capacity;
    protected int income;

    protected TextMesh userText;
    protected TextMesh incomeText;

    // Happiness sprites
    SpriteRenderer dead;
    SpriteRenderer happy;
    SpriteRenderer veryHappy;
    SpriteRenderer passive;
    SpriteRenderer angry;

    protected override void Start()
    {
        base.Start();
        tracker = gameObject.GetComponent<ItemTracker>();
    }

    protected override void UpdateReferences()
    {
        userText = tooltip.transform.Find("UserText").GetComponent<TextMesh>();
        incomeText = tooltip.transform.Find("IncomeText").GetComponent<TextMesh>();

        dead = tooltip.transform.Find("Icons/Dead").GetComponent<SpriteRenderer>();
        happy = tooltip.transform.Find("Icons/Happy").GetComponent<SpriteRenderer>();
        angry = tooltip.transform.Find("Icons/Angry").GetComponent<SpriteRenderer>();
        passive = tooltip.transform.Find("Icons/Passive").GetComponent<SpriteRenderer>();
        veryHappy = tooltip.transform.Find("Icons/VeryHappy").GetComponent<SpriteRenderer>();
    }

    protected override void UpdateText()
    {
        userText.text = tracker.users.ToString() + " / " + capacity.ToString();
        incomeText.text = income.ToString();
        if (income > 0)
            incomeText.color = ReferenceManager.instance.moneyGreen.color;
        else incomeText.color = ReferenceManager.instance.moneyRed.color;
    }

    protected override void UpdateValues()
    {
        if (!tracker)
            //ResetTracker();
        users = tracker.users;
        capacity = tracker.capacity;
        income = (int)tracker.income;
        base.UpdateValues();
        UpdateSprites();
    }

    protected virtual void ResetTracker()
    {
        tracker = gameObject.GetComponent<ItemTracker>();
    }

    protected virtual void UpdateSprites()
    {
        GetHappiness();
    }

    void GetHappiness()
    {
        int newHappiness = tracker.happinessState;
        SetHappiness(newHappiness);
    }

    void SetHappiness(int newHappiness)
    {
        DisableSprites();
        if (newHappiness == 0)
        {
            dead.enabled = true;
        }
        else if (newHappiness == 1)
        {
            angry.enabled = true;
        }
        else if (newHappiness == 2)
        {
            passive.enabled = true;
        }
        else if (newHappiness == 3)
        {
            happy.enabled = true;
        }
        else if (newHappiness == 4)
        {
            veryHappy.enabled = true;
        }
    }

    void DisableSprites()
    {
        if (dead == null || happy == null || veryHappy == null || passive == null || angry == null)
        {
            UpdateReferences();
        }
        dead.enabled = false;
        happy.enabled = false;
        veryHappy.enabled = false;
        passive.enabled = false;
        angry.enabled = false;
    }
}
