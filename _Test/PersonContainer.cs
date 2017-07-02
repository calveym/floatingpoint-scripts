using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersonContainer : MonoBehaviour
{
    public int capacity;
    public int users;


    public void AddUsers(int numUsers)
    {
        if (numUsers + users <= capacity)
            users += numUsers;
    }

    public void RemoveUsers(int numUsers)
    {
        if (users - numUsers >= 0)
            users -= numUsers;
    }

    public bool Full()
    {
        if (users == capacity)
            return true;
        else return false;
    }

    public bool Empty()
    {
        if (capacity == 0)
            return true;
        else return false;
    }
}
