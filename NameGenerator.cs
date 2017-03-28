using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour {

    List<string> firstNames;
    List<string> lastNames;

    void Awake()
    {
        firstNames = new List<string>();
        lastNames = new List<string>();
        SetupNames();
    }

    void SetupNames()
    {
        AddToList(firstNames, "Morty", "James", "Monica", "Terry", "Geraldine", "Edwina", "William", "Richard", "Jessica");
        AddToList(lastNames, "Williams", "Morton", "McCloud", "Fliman", "Audley", "Richards");
    }

	public List<string> FirstNames()
    {
        return firstNames;
    }

    public List<string> LastNames()
    {
        return lastNames;
    }

    void AddToList(List<string> someStringList, params string[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            someStringList.Add(list[i]);
        }
    }
}
