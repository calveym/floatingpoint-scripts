using VRTK;
using UnityEngine;

public class SpawnerCube : VRTK_InteractableObject
{
    DisplayMenu displayMenu;
    string modelName;

    private void Start()
    {
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        modelName = gameObject.name;
    }

	public override void StartUsing (GameObject usingObject) {
        base.StartUsing(usingObject);

        ChooseModel();
	}

    void ChooseModel()
    {
        if (modelName == "Model1")
        {
            displayMenu.InitiateSpawn(gameObject);
        }
        else if (modelName == "Model2")
        {
            displayMenu.InitiateSpawn(gameObject);
        }
        else if (modelName == "Model3")
        {
            displayMenu.InitiateSpawn(gameObject);
        }
        else if (modelName == "Model4")
        {
            displayMenu.InitiateSpawn(gameObject);
        }
        else if (modelName == "Model5")
        {
            displayMenu.InitiateSpawn(gameObject);
        }
    }
}
