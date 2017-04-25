using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    private void Awake()
    {
        LoadAllBuildings();
    }

    // Small size residential buildings
    Object[] res;
    Object[] com;
    Object[] ind;
    Object[] off;
    Object[] leis;
    Object[] util;
    Object[] indc;
    Object[] fol;

    public void Spawn(Vector3 targetPosition, int type, int unit)
    // Starts spawn process, enables static spawn calls
    {
        GameObject newBuilding = Instantiate(FindBuilding(type, unit), targetPosition, Quaternion.identity) as GameObject;
    }

    Object FindBuilding(int type, int unit)
    {
        type = 0;
        switch (type)
        {
            case 0:
                return res[unit];
            case 1:
                return com[unit];
            case 2:
                return ind[unit];
            case 3:
                return off[unit];
            case 4:
                return leis[unit];
            case 5:
                return util[unit];
            case 6:
                return indc[unit];
            case 7:
                return fol[unit];
        }
        return new GameObject();
    }

    void LoadAllBuildings()
    // Loads buildings from resources folder
    {
        res = Resources.LoadAll("res");
        com = Resources.LoadAll("com");
        ind = Resources.LoadAll("ind");
        off = Resources.LoadAll("off");
        leis = Resources.LoadAll("leis");
        util = Resources.LoadAll("util");
        indc = Resources.LoadAll("indc");
        fol = Resources.LoadAll("fol");
    }

}
