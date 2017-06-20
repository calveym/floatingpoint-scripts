using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersonContainer
{
    int capacity { get; set;}
    int users { get; set; }

    void AddUsers(int numUsers);
    void RemoveUsers(int numUsers);

    bool Full();
    bool Empty();
}
