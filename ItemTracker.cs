using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    // Declare variables
    PopulationManager populationManager;
    EconomyManager economyManager;
    
    public string type;
    public int capacity;
    public float income;
    public int users;

    private void Start()
    // Sets start variables
    {
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();

        income = 0;
        users = 0;
        type = null;
        capacity = 1;
    }

    private void Update()
    {
        UpdateValues();
        RunChecks();
    }

    void UpdateValues()
    {
        income = users * (1 + 0.01f * economyManager.residentialTaxRate);
    }

    void RunChecks()
    // Runs checks to make sure current state is legal
    {
        OverCapacity();
        EnablePhysics();
    }

    void EnablePhysics()
    // Enables physics if object has no parent
    {
        if (transform.parent == null && gameObject.tag != "road")
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void DeallocateUsers(int numUsers)
    // Returns unallocated users to the populationManager
    {
        populationManager.DeallocateUsers(numUsers);
        users -= numUsers;
    }

    void OverCapacity()
    // Returns true if more users than capacity
    {
        if (users > capacity)
        {
            DeallocateUsers(capacity - users);
        }
    }

    public void setValues(string typeInput, int pressedButton)
    // Type setter
    {
        type = typeInput;
		capacity = (pressedButton * pressedButton) + 1;
    }

    public void AddUsers(int numUsers)
    // Adds numUsers to users if capacity is not exceeded
    {
        if(numUsers + users < capacity)
        {
            users += numUsers;
        }
    }

    public void RemoveUsers()
    // Removes all local and related itemManager users
    {
        populationManager.unallocatedPopulation += users;
        populationManager.population -= users;
        users = 0;
    }

    public int NumVacancies()
    // Returns number of available spaces
    {
        return capacity - users;
    }

    public int GetCapacity()
    // Returns building capacity
    {
        return capacity;
    }
}
