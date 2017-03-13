using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour {

    static List<string> firstNames;
    static List<string> lastNames;

    private void Awake()
    {
        firstNames = new List<string>();
        lastNames = new List<string>();
        SetupNames();
    }

    static void SetupNames()
    {
        AddToList(firstNames, "Morty", "James", "Monica", "Terry", "Geraldine", "Edwina", "William", "Richard");
        AddToList(lastNames, "Williams", "Morton", "McCloud", "Fliman", "Audley", "Richards");
    }

	public static List<string> FirstNames()
    {
        return firstNames;
    }

    public static List<string> LastNames()
    {
        return lastNames;
    }

    static void AddToList(List<string> someStringList, params string[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            someStringList.Add(list[i]);
        }
    }
}
