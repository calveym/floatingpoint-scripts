using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialTracker : ItemTracker {
    // Manages individual stats of each industrial building.

    [Tooltip("The amount each worker must produce to be happy")]
    public int requiredProduction = 5; // How much each individual must produce to be satisfied
    System.Random rand;

    GameObject markerPrefab;
    Marker marker;
    List<IndustrialComponent> components;
    List<float> sales; // List of recent sales counted in goodsSold
    List<ResidentialTracker> employees;

    public float productionHappiness; // Happiness from reaching sales targets

    public int visitors;
    public int lifetimeVisitors;

    public static float allGoods; // Base goods tracking figure for each economic tick

    public float sellPrice = 1;  // Base sell price

    // Multipliers from components
    public float productionMulti;

    bool checkEnable;


    void Awake()
    {
        rand = new System.Random(); //reuse this if generating many
        sellPrice = 1;
        employees = new List<ResidentialTracker>();
        sales = new List<float>();
        components = new List<IndustrialComponent>();
        goodsSold = 0;
        productionMulti = 1;
        StartCoroutine("CheckEnable");
    }

    new void Start()
    {
        base.Start();
        markerPrefab = GameObject.Find("MarkerPrefab");
    }

    void Update()
    {
        if (!updateStarted && usable && validPosition)
        {
            EnableSnap();
            updateStarted = true;
            EconomyManager.ecoTick += UpdateSecond;
            ReferenceManager.instance.itemManager.addIndustrial(capacity, gameObject);
        }
        else if (updateStarted && !usable || !validPosition)
        {
            updateStarted = false;
            EconomyManager.ecoTick -= UpdateSecond;
        }
        else if (!usable && !updateStarted && validPosition)
        {
            checkEnable = true;
        }
    }

    void ProduceGoods()
    {
        goodsProduced = users * productionMulti * (longtermHappiness / 50);

        income = goodsProduced * sellPrice * (1 + (landValue * 0.01f)); //* ReferenceManager.instance.industrialIncomeMultiplier;
        //Debug.Log("Income: " + goodsProduced * sellPrice * (1 + (landValue * 0.01f)));
        income -= baseCost;
        allGoods += goodsProduced;
    }

    public void Apply(float applicantLandValue, ResidentialTracker applicantTracker)
    {
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log((float)u1)) *
                     Mathf.Sin(2.0f * Mathf.PI * (float)u2); //random normal(0,1)
        double randNormal =
                     landValue - 5 + 5 * randStdNormal; //random normal(mean,stdDev^2)
        if (usable && users < capacity) // TODO: ADD LAND VALUE CHECKS BACK IN
        {
            AcceptApplication(applicantTracker);
        }
        else RejectApplication(applicantTracker);
    }

    void AcceptApplication(ResidentialTracker applicantTracker)
    {
        if (applicantTracker.unemployedPopulation >= 1)
        {
            AddUsers(1);
            employees.Add(applicantTracker);
            applicantTracker.AcceptApplication();
        }
        else RejectApplication(applicantTracker);
    }

    void RejectApplication(ResidentialTracker applicantTracker)
    {
        // TODO:
        //Debug.Log("Applicant rejected!!!");
    }

    public void RemoveAllUsers()
    {
        foreach (ResidentialTracker res in employees)
        {
            res.unemployedPopulation++;
        }
    }

    public void AddMarker()
    {
        marker = Instantiate(markerPrefab, transform.position + new Vector3(0f, 2f, 0f), transform.rotation, transform).GetComponent<Marker>();
        marker.transform.localScale *= 10;
        marker.StartRotation();
    }

    public void LinkComponent(IndustrialComponent component)
    // Sent from component, completes link
    {
        if(!components.Contains(component))
        {
            components.Add(component);
            productionMulti += component.productionMulti;
        }
    }

    public void UnlinkComponent(IndustrialComponent component)
    {
        if(components.Contains(component))
        {
            components.Remove(component);
            productionMulti -= component.productionMulti;
        }
    }

    void UpdateProductionHappiness()
    {
        if (capacity != 0 && requiredProduction != 0)
        {
            productionHappiness = (goodsProduced / capacity * requiredProduction) * 40;
            if (productionHappiness > 40)
            {
                productionHappiness = 40; // Capped at 40
            }
        }
        else productionHappiness = 0;
    }

    void UpdateSecond()
    // Updates values once per second, economic tick
    {
        updateStarted = true;
        if (!usable || !validPosition)
        {
            return;
        }
        UpdateLocalHappiness();
        UpdateProductionHappiness();
        UpdateHappiness();
        UpdateLandValue();
        UpdateTransportationValue();
        ProduceGoods();
        totalIndustrialIncome += income;
    }

    void UpdateHappiness()
    // Performs all necessary final happiness calculations, including longterm
    {
        currentHappiness = localHappiness + productionHappiness + fillRateHappiness;
        CalculateLongtermHappiness();
        CalculateHappinessState();
    }

    public string ValidPosition()
    {
        if (validPosition)
        {
            return "Active";
        }
        else return "Inactive";
    }

    public string FancyIncome()
    {
        return "Income: $" + Mathf.Round(income * 100) / 100 + "/w";
    }

    public string FancyCapacity()
    {
        return "Workers: " + users + " / " + capacity;
    }

    public int FancyHappiness()
    {
        return happinessState;
    }
    
    public string FancyTitle()
    {
        // TODO: NICE TITLES
        return buildingName;
    }

    public string FancyGoods()
    {
        return "Goods sold: " + goodsSold.ToString();
    }

    public string FancyLandValue()
    {
        return "Land Value: $" + landValue;
    }
    public void MarkWithComponent()
    {
        StartCoroutine("MarkWithComponent");
    }

    IEnumerator CheckEnable()
    {
        while (true)
        {
            if (checkEnable)
            {
                checkEnable = false;
                if (transform.parent == null)
                {
                    usable = true;
                }
            }
            yield return new WaitForSeconds(5);
        }
    }
}
